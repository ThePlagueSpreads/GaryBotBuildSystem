using Core.Config;
using Microsoft.Extensions.Options;

namespace Core;

public class WorkspaceManager(IOptions<PathOptions> options)
{
    private const string BuildsFolderName = "Builds";
    
    public string GetBuildsFolderPath()
    {
        var path = Path.Combine(GetWorkspacePath(), BuildsFolderName);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    private string GetWorkspacePath()
    {
        var workspace = options.Value.WorkspacePath;
        if (string.IsNullOrEmpty(workspace) || !Directory.Exists(workspace))
        {
            throw new DirectoryNotFoundException("Invalid workspace path.");
        }
        return workspace;
    }

    public string CreateUploadZipFilePath(string shortFolderName, BuildRequest request)
    {
        var folder = GetBuildsFolderPath();
        var timeString = request.RequestedAt.ToString("yyyy-MM-dd-HH-mm-ss");
        var fileName = $"{shortFolderName}-{timeString}.zip";
        return Path.Combine(folder, fileName);
    }
}