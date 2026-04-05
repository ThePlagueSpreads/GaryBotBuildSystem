using Core;

namespace GaryBotServer.Discord;

public interface IBuildNotifications
{
    Task NotifyBuildQueuedAsync(BuildRequest request, CancellationToken ct = default);
    Task NotifyBuildCompletedAsync(BuildRequest request, CancellationToken ct = default);
    Task NotifyBuildFailedAsync(BuildRequest request, Exception ex, CancellationToken ct = default);
}