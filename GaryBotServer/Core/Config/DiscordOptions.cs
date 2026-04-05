namespace Core.Config;

public class DiscordOptions
{
    public string Token { get; set; } = string.Empty;
    public ulong GuildId { get; set; }
    public ulong BuildChannelId { get; set; }
    public ulong RoleId { get; set; }
}