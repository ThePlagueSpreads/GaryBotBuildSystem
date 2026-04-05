namespace Core.Config;

public sealed class UploadOptions
{
    public required string[] FolderNames { get; init; }
    public required string FolderId { get; init; }
}