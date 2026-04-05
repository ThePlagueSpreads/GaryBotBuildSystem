namespace Core.Config;

public sealed class ScheduleOptions
{
    public required bool BuildOnStartUp { get; init; } = true;
    public required int StartUpBuildDelaySeconds { get; init; } = 3;
    public required int BuildIntervalMinutes { get; init; } = 1440;
}