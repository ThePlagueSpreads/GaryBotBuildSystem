using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public class PullAssetsStep(IOptions<GitHubOptions> options) : PullRepositoryStep("Assets", options.Value.AssetsRepoPath);