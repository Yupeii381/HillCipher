using System;

namespace Client.Models;

public record HistoryItem
{
    public int Id { get; init; }
    public string Input { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public string Mode { get; init; } = "encrypt";
    public string Output { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; }
}