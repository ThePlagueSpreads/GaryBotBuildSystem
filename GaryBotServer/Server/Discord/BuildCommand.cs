using Core;
using Discord.Interactions;

namespace GaryBotServer.Discord;

public class BuildCommand(IBuildQueue buildQueue) : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("buildtrp", "Queue a new build of The Red Plague")]
    public async Task BuildAsync()
    {
        var request = new BuildRequest
        {
            RequestedBy = $"<@{Context.User.Id}> ({Context.User.Username})"
        };

        await buildQueue.EnqueueAsync(request);

        await RespondAsync(
            $"Queued build `{request.Id}` requested by {Context.User.Mention}.",
            ephemeral: false);
    }
}