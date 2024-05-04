using HackerNewsApi.DTOs;

namespace HackerNewsApi.Services;

public interface IHackerNewsService
{
    Task<IEnumerable<HackerNewsItem>> GetTopItemsAsync(int n);

    Task<IEnumerable<HackerNewsItem>> GetTopStories();
}