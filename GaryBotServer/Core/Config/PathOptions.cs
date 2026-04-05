namespace Core.Config;

public sealed class PathOptions
{
    public required string UnityExePath { get; init; }
    public required string WorkspacePath { get; init; }
    public required string UnityProjectPath { get; init; } 
    public required string SolutionDirectory { get; init; } 
    public required string BuildOutputPath { get; init; } 
    public required string LogFilePath { get; init; } 
}