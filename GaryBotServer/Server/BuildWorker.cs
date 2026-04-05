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
        CancellationToken ct)
    {
        _building = true;
        _currentBuild = request;
        
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

        _building = false;
        _currentBuild = null;
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
}