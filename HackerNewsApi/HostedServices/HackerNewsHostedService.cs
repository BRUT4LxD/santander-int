using HackerNewsApi.Constants;
using HackerNewsApi.DTOs;
using HackerNewsApi.Services;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsApi.HostedServices;

public class HackerNewsHostedService : IHostedService
{
    private readonly IHackerNewsService _hackerNewsService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<HackerNewsHostedService> _logger;
    private Timer _timer;
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    public HackerNewsHostedService(IHackerNewsService hackerNewsService, IMemoryCache cache, ILogger<HackerNewsHostedService> logger)
    {
        _hackerNewsService = hackerNewsService;
        _cache = cache;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HackerNewsHostedService is starting.");

        _cache.Set(CacheKeys.BestStories, Enumerable.Empty<HackerNewsItem>());

        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, Interval);
        return Task.CompletedTask;
    }

    private async void TimerCallback(object state)
    {
        _logger.LogInformation("Getting Top Stories");

        try
        {
            var topItems = await _hackerNewsService.GetTopStories();
            _cache.Set(CacheKeys.BestStories, topItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred getting top stories");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HackerNewsHostedService is stopping.");

        _timer.Change(Timeout.Infinite, 0);
        _timer.Dispose();

        return Task.CompletedTask;
    }
}
