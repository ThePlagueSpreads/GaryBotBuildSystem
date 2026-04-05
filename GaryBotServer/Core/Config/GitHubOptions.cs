namespace Core.Config;

public sealed class GitHubOptions
{
    public required string CodeRepoPath { get; init; }
    public required string AssetsRepoPath { get; init; }
}