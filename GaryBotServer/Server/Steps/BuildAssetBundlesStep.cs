using CliWrap;
using CliWrap.Buffered;
using Core;
using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public sealed class BuildAssetBundlesStep(IOptions<PathOptions> options, IOptions<PlatformOptions> platformOptions) : IBuildStep
{
    public string Name => $"Build Unity AssetBundles ({options.Value.UnityProjectPath})";

    public async Task ExecuteAsync(BuildRequest request, CancellationToken ct)
    {
        request.Status = "Building Asset Bundles";
        
        try
        {
            var result = await Cli.Wrap(options.Value.UnityExePath)
                .WithArguments(args => args
                    .Add("-batchmode")
                    .Add("-quit")
                    .Add("-projectPath").Add(options.Value.UnityProjectPath)
                    .Add("-executeMethod").Add("BuildAssetBundles.BuildFromCommandLine")
                    .Add("-bundleOutput")
                    .Add($"AssetBundles/{GetBestOutputFolderForPlatform(platformOptions.Value.Platform)}")
                    .Add("-bundleTarget").Add(platformOptions.Value.Platform)
                    .Add("-logFile").Add(options.Value.LogFilePath))
                .ExecuteBufferedAsync(ct);
            
            if (result.ExitCode != 0)
                throw new Exception($"AssetBundle build failed with exit code {result.ExitCode}.");
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            // Don't silently cancel
            request.Status = "Canceled";
            Console.WriteLine("Unity build canceled.");
            throw;
        }
    }

    private string GetBestOutputFolderForPlatform(string platform)
    {
        if (platform.Contains("windows", StringComparison.OrdinalIgnoreCase))
            return "StandaloneWindows";
        if (platform.Contains("osx", StringComparison.OrdinalIgnoreCase))
            return "StandaloneOSXUniversal";
        if (platform.Contains("linux", StringComparison.OrdinalIgnoreCase))
            return "StandaloneLinux64";
        throw new ArgumentException("Unknown platform: " + platform);
    }
}