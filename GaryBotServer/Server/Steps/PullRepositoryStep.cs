using CliWrap;
using Core;

namespace GaryBotServer.Steps;

public abstract class PullRepositoryStep(string repoName, string repoPath) : IBuildStep
{
    public string Name => $"Pull from GitHub ({repoPath})";
    
    public async Task ExecuteAsync(BuildRequest request, CancellationToken ct)
    {
        request.Status = $"Fetching from {repoName} repository";
        
        await Cli.Wrap("git")
            .WithArguments(["fetch", "--all", "--prune"])
            .WithWorkingDirectory(repoPath)
            .ExecuteAsync(ct);

        request.Status = $"Pulling from {repoName} repository";
        
        await Cli.Wrap("git")
            .WithArguments(["pull"])
            .WithWorkingDirectory(repoPath)
            .ExecuteAsync(ct);
    }
}