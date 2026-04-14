using Discord.Interactions;

namespace GaryBotServer.Discord;

public class StopCommand : InteractionModuleBase<SocketInteractionContext>
{
    private const int GiveUpSeconds = 30;
    
    [SlashCommand("stop", "Ends any tasks on the current GaryBot instance")]
    public async Task StopAsync(bool clearQueue = true)
    {
        var worker = BuildWorker.Singleton;
        if (worker == null)
        {
            await RespondAsync(
                "No worker detected!",
                ephemeral: true);
            return;
        }

        var cancelResult = new BuildWorker.CancelResult();
        bool canceled = worker.CancelBuild(clearQueue, cancelResult);

        if (!canceled)
        {
            await RespondAsync(
                "Found nothing to cancel.",
                ephemeral: true);
        }

        bool cancelComplete = false;
        DateTime giveUpTime = DateTime.UtcNow.AddSeconds(GiveUpSeconds);
        while (!cancelComplete && giveUpTime < DateTime.UtcNow)
        {
            await Task.Delay(100);
            cancelComplete = cancelResult.FinishedCancelling;
        }

        if (cancelComplete)
        {
            await RespondAsync(
                "Build canceled successfully.",
                ephemeral: false);
        }
        else
        {
            await RespondAsync(
                "Timed out while canceling.",
                ephemeral: false);
        }
    }
}