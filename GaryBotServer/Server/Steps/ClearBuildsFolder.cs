using Core;

namespace GaryBotServer.Steps;

public sealed class ClearBuildsFolder(WorkspaceManager workspace) : IBuildStep
{
    public string Name => "Clear Builds Folder";
    public Task ExecuteAsync(BuildRequest request, CancellationToken ct)
    {
        request.Status = "Preparing workspace";
        
        var directory = workspace.GetBuildsFolderPath();
        var files = Directory.GetFiles(directory);
        if (files.Length == 0)
        {
            return Task.CompletedTask;
        }

        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".zip")
            {
                File.Delete(file);
            }
            else
            {
                Console.WriteLine($"Warning: non-zip file detected in builds folder ({file})");
            }
        }

        return Task.CompletedTask;
    }
}