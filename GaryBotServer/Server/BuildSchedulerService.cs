using Core;
using Core.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GaryBotServer;

public class BuildSchedulerService(IBuildQueue queue, IOptions<ScheduleOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(options.Value.StartUpBuildDelaySeconds * 1000, stoppingToken);

        if (options.Value.BuildOnStartUp)
        {
            await queue.EnqueueAsync(new BuildRequest
            {
                RequestedBy = "Scheduler (startup build)"
            }, stoppingToken);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(options.Value.BuildIntervalMinutes), stoppingToken);
            if (!stoppingToken.IsCancellationRequested)
            {
                await queue.EnqueueAsync(new BuildRequest
                {
                    RequestedBy = "Scheduler (automatic build)"
                }, stoppingToken);
            }
        }
    }
}