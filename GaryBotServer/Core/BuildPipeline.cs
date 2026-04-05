namespace Core;

public sealed class BuildPipeline
{
    private readonly IReadOnlyList<IBuildStep> _steps;

    public BuildPipeline(IEnumerable<IBuildStep> steps)
        => _steps = steps.ToList();

    public async Task RunAsync(
        BuildRequest request,
        CancellationToken ct)
    {
        foreach (var step in _steps)
        {
            Console.WriteLine($"> {step.Name}");
            await step.ExecuteAsync(request, ct);
        }
    }
}