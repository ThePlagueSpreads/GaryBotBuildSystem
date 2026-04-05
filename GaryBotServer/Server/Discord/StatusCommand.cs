using Discord.Interactions;

namespace GaryBotServer.Discord;

public class StatusCommand() : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("status", "Gets the status of the current GaryBot instance")]
    public async Task BuildAsync()
    {
        if (BuildWorker.Singleton == null)
        {
            await RespondAsync(
                "**Status Report**\nBooting up...",
                ephemeral: false);
            return;
        }
        
        await RespondAsync(
            $"**Status Report**\nHello! I'm online right now.\n-# Status: {BuildWorker.Singleton.GetCurrentStatus()}",
            ephemeral: false);
    }
}