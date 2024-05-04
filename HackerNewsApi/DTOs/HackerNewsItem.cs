using System.Text.Json.Serialization;

namespace HackerNewsApi.DTOs;

public class HackerNewsItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("by")]
    public string By { get; set; }

    [JsonPropertyName("time")]
    public int Time { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("descendants")]
    public int CommentCount { get; set; }
}
