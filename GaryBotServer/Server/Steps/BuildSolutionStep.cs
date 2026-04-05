using CliWrap;
using Core;
using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public class BuildSolutionStep(IOptions<PathOptions> options) : IBuildStep
{
    public string Name => "Build Solution";
    
    public async Task ExecuteAsync(BuildRequest request, CancellationToken ct)
    {
        request.Status = "Building Solution";
        
        await Cli.Wrap("dotnet")
            .WithArguments(["build"])
            .WithWorkingDirectory(options.Value.SolutionDirectory)
            .ExecuteAsync(ct);
    }
}