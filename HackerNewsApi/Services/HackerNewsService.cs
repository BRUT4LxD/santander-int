using System.Text.Json;
using HackerNewsApi.Constants;
using HackerNewsApi.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsApi.Services;

internal class HackerNewsService : IHackerNewsService
{
    private const string BaseUrl = "https://hacker-news.firebaseio.com/v0";
    private readonly IHttpClientFactory _clientFactory;
    private readonly IMemoryCache _cache;

    public HackerNewsService(IHttpClientFactory clientFactory, IMemoryCache cache)
    {
        _clientFactory = clientFactory;
        _cache = cache;
    }

    public async Task<IEnumerable<HackerNewsItem>> GetTopItemsAsync(int n)
    {
        if (_cache.TryGetValue(CacheKeys.BestStories, out IEnumerable<HackerNewsItem>? items) && items?.Any() == true)
        {
            return items.Take(n);
        }

        var topItems = await GetTopStories();
        _cache.Set(CacheKeys.BestStories, topItems);

        return topItems.Take(n);
    }

    private async Task<HackerNewsItem> GetItemAsync(int id)
    {
        var client = _clientFactory.CreateClient();
        var url = $"{BaseUrl}/item/{id}.json";
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<HackerNewsItem>(json)!;
    }

    public async Task<IEnumerable<HackerNewsItem>> GetTopStories()
    {
        var client = _clientFactory.CreateClient();

        var url = $"{BaseUrl}/beststories.json";
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var topItems = JsonSerializer.Deserialize<int[]>(await response.Content.ReadAsStringAsync());

        if (topItems is null)
        {
            return Enumerable.Empty<HackerNewsItem>();
        }

        const int maxConcurrentCalls = 20;

        var semaphore = new SemaphoreSlim(maxConcurrentCalls);

        var tasks = topItems.Select(async id =>
        {
            await semaphore.WaitAsync();
            try
            {
                return await GetItemAsync(id);
            }
            finally
            {
                semaphore.Release();
            }
        });

        var result = await Task.WhenAll(tasks);

        return result.OrderByDescending(x => x.Score);
    }
}
