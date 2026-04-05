using System.IO.Compression;
using Core;
using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public sealed class ZipFoldersStep(IOptions<PathOptions> pathOptions, IOptions<UploadOptions> uploadOptions, WorkspaceManager workspace) : IBuildStep
{
    public string Name => "Zip folders";
    
    public async Task ExecuteAsync(BuildRequest request, CancellationToken ct)
    {
        request.Status = "Zipping folders";
        foreach (var folder in uploadOptions.Value.FolderNames)
        {
            var fullFolderPath = Path.Combine(pathOptions.Value.BuildOutputPath, folder);
            request.Status = $"Zipping folder: {folder}";
            Console.WriteLine($"Zipping folder '{folder}' ({fullFolderPath})");
            await ZipFile.CreateFromDirectoryAsync(fullFolderPath, workspace.CreateUploadZipFilePath(folder, request), ct);
        }
    }
}