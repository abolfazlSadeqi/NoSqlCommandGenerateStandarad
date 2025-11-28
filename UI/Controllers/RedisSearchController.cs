using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisSearchController : Controller
{
    private readonly IRedisSearchService _service;

    public RedisSearchController(IRedisSearchService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisSearchViewModel());

    [HttpPost]
    public IActionResult Index(RedisSearchViewModel model, string actionType)
    {
        switch (actionType)
        {
            case "SCAN":
                model.GeneratedScript = _service.Scan(model.Pattern, model.Count);
                break;
            case "Pattern":
                model.GeneratedScript = _service.FilterMatch(model.Pattern);
                break;
            case "Filter":
                model.GeneratedScript = _service.FilterBuilder(model.Pattern, model.Filter);
                break;
        }

        return View(model);
    }
}
