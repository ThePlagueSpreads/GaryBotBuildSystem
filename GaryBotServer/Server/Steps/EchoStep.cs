using CliWrap;
using Core;
using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public sealed class EchoStep(IOptions<PathOptions> options) : IBuildStep
{
    public string Name => "Echo";

    public async Task ExecuteAsync(
        BuildRequest request,
        CancellationToken ct)
    {
        await Cli.Wrap("cmd")
            .WithArguments("/c echo Building " + request.Id + " at path " + options.Value.WorkspacePath)
            .ExecuteAsync(ct);
    }
}