namespace Core;

public interface IBuildQueue
{
    Task EnqueueAsync(BuildRequest request, CancellationToken ct = default);
    IAsyncEnumerable<BuildRequest> ReadAllAsync(CancellationToken ct = default);
    bool Clear();
}