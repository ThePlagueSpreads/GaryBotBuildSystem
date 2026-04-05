using Core;
using Core.Config;
using GaryBotServer;
using GaryBotServer.Discord;
using GaryBotServer.Steps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // Create the queue instance
        var buildQueue = new SerialBuildQueue();

        // Create the host
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IBuildQueue>(buildQueue);
                services.Configure<PathOptions>(
                    context.Configuration.GetSection("Paths"));
                services.Configure<GitHubOptions>(
                    context.Configuration.GetSection("GitHubOptions"));
                services.Configure<PlatformOptions>(
                    context.Configuration.GetSection("PlatformOptions"));
                services.Configure<UploadOptions>(
                    context.Configuration.GetSection("UploadOptions"));
                services.Configure<ScheduleOptions>(
                    context.Configuration.GetSection("ScheduleOptions"));
                services.Configure<DiscordOptions>(
                    context.Configuration.GetSection("DiscordOptions"));

                services.AddSingleton<WorkspaceManager>();

                // BUILD STEPS
                services.AddSingleton<IBuildStep, ClearBuildsFolder>();
                services.AddSingleton<IBuildStep, PullAssetsStep>();
                services.AddSingleton<IBuildStep, PullCodeStep>();
                services.AddSingleton<IBuildStep, BuildAssetBundlesStep>();
                services.AddSingleton<IBuildStep, BuildSolutionStep>();
                services.AddSingleton<IBuildStep, ZipFoldersStep>();
                services.AddSingleton<IBuildStep, UploadBuildsStep>();
                
                // Other
                
                services.AddSingleton<BuildPipeline>();
                
                services.AddHostedService<BuildWorker>();
                services.AddHostedService<BuildSchedulerService>();
                
                // Add Discord integration... cursed I know
                
                services.AddSingleton<DiscordBotService>();

                services.AddSingleton<IBuildNotifications>(sp =>
                    sp.GetRequiredService<DiscordBotService>());

                services.AddHostedService(sp =>
                    sp.GetRequiredService<DiscordBotService>());
            })
            .Build();

        // Start the host
        await host.StartAsync();

        Console.WriteLine("Build server running...");

        Console.WriteLine("Press enter to exit the program...");
        Console.ReadLine();
        Console.WriteLine("Press enter again to exit.");
        Console.ReadLine();
    }
}