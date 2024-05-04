using HackerNewsApi.Constants;
using HackerNewsApi.HostedServices;
using HackerNewsApi.Middlewares;
using HackerNewsApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
});

builder.Services.AddHostedService<HackerNewsHostedService>();
builder.Services.AddSingleton<IHackerNewsService, HackerNewsService>();
builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder =>
    {
        builder.Expire(TimeSpan.FromMinutes(1));
    });

    options.AddPolicy(Policies.HackerNewsApiPolicy, builder =>
    {
        builder.Expire(TimeSpan.FromMinutes(1));
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseOutputCache();


app.MapGet("/hackernews/{n:int}", async (int n, IHackerNewsService hackerNewsService) =>
{
    var items = await hackerNewsService.GetTopItemsAsync(n);
    return Results.Ok(items);
})
.CacheOutput(policyName: Policies.HackerNewsApiPolicy);

app.Run();
