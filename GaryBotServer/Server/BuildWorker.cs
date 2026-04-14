using Core;
using GaryBotServer.Discord;

namespace GaryBotServer;

using Microsoft.Extensions.Hosting;

public sealed class BuildWorker : BackgroundService
{
    public static BuildWorker? Singleton { get; private set; }
    private bool _building;
    private BuildRequest? _currentBuild;
    private readonly IBuildQueue _queue;
    private readonly BuildPipeline _pipeline;
    private readonly IBuildNotifications _notifications;
    
    private CancellationTokenSource? _currentBuildCancellation;

    private List<CancelResult> _cancelResults = [];

    public BuildWorker(IBuildQueue queue,
        BuildPipeline pipeline,
        IBuildNotifications notifications)
    {
        Singleton = this;
        _queue = queue;
        _pipeline = pipeline;
        _notifications = notifications;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var request in _queue.ReadAllAsync(stoppingToken))
        {
            await RunBuildAsync(request, stoppingToken);
        }
    }

    private async Task RunBuildAsync(
        BuildRequest request,
        CancellationToken stoppingToken)
    {
        _building = true;
        _currentBuild = request;
        
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        _currentBuildCancellation = cts;
        
        var ct = cts.Token;
        
        await _notifications.NotifyBuildQueuedAsync(request, ct);
        Console.WriteLine($"Starting build {request.Id}");

        try
        {
            await _pipeline.RunAsync(request, ct);
            Console.WriteLine($"Build {request.Id} succeeded");
            await _notifications.NotifyBuildCompletedAsync(request, ct);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync($"Build {request.Id} failed: {ex}");
            await _notifications.NotifyBuildFailedAsync(request, ex, ct);
        }
        finally
        {
            _currentBuildCancellation = null;
            _building = false;
            _currentBuild = null;
            UpdateCancelResults();
        }
    }

    private void UpdateCancelResults()
    {
        foreach (var result in _cancelResults)
        {
            result.FinishedCancelling = true;
        }
        _cancelResults.Clear();
    }

    public string GetCurrentStatus()
    {
        if (!_building)
        {
            return "Idle";
        }

        if (_currentBuild == null)
        {
            return "Stuck";
        }

        return "Working - " + _currentBuild.Status;
    }

    public bool CancelBuild(bool clearQueue, CancelResult result)
    {
        _cancelResults.Add(result);
        
        bool canceled = false;
        
        if (_building && _currentBuildCancellation != null)
        {
            _currentBuildCancellation.Cancel();
            canceled = true;
        }

        if (clearQueue && _queue.Clear())
        {
            canceled = true;
        }
        
        return canceled;
    }

    public sealed class CancelResult
    {
        public bool FinishedCancelling { get; set; }
    }
}