using HackerNewsApi.Constants;
using HackerNewsApi.DTOs;
using HackerNewsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HackerNewsApi.Controllers;

[ApiController]
public class HackerNewsController : ControllerBase
{
    private readonly IHackerNewsService _hackerNewsService;

    public HackerNewsController(IHackerNewsService hackerNewsService)
    {
        _hackerNewsService = hackerNewsService;
    }


    [HttpGet("[controller]/{n:int}")]
    [OutputCache(PolicyName = Policies.HackerNewsApiPolicy)]
    public async Task<ActionResult<IEnumerable<HackerNewsItem>>> Get(int n)
    {
        var items = await _hackerNewsService.GetTopItemsAsync(n);

        return Ok(items);
    }

}
