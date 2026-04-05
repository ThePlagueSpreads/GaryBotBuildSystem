using Core.Config;
using Microsoft.Extensions.Options;

namespace GaryBotServer.Steps;

public class PullCodeStep(IOptions<GitHubOptions> options) : PullRepositoryStep("Code", options.Value.CodeRepoPath);