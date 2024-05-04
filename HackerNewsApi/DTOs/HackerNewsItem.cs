using System.Text.Json.Serialization;

namespace HackerNewsApi.DTOs;

public readonly struct HackerNewsItem
{
    [JsonPropertyName("title")]
    public string Title { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; }

    [JsonPropertyName("by")]
    public string By { get; init; }

    [JsonPropertyName("time")]
    public int Time { get; init; }

    [JsonPropertyName("score")]
    public int Score { get; init; }

    [JsonPropertyName("descendants")]
    public int CommentCount { get; init; }
}
