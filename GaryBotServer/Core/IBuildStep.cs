namespace Core;

public interface IBuildStep
{
    string Name { get; }
    Task ExecuteAsync(BuildRequest request, CancellationToken ct);
}