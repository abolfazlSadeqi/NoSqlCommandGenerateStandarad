using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisListController : Controller
{
    private readonly IRedisListService _service;

    public RedisListController(IRedisListService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisListOperationViewModel());

    [HttpPost]
    public IActionResult Index(RedisListOperationViewModel model, string actionType)
    {
        switch (actionType)
        {
            case "LPUSH":
                model.GeneratedScript = _service.LPush(model.Key, model.Value);
                break;
            case "RPUSH":
                model.GeneratedScript = _service.RPush(model.Key, model.Value);
                break;
            case "LPOP":
                model.GeneratedScript = _service.LPop(model.Key);
                break;
            case "LRANGE":
                if (!model.Start.HasValue) model.Start = 0;
                if (!model.Stop.HasValue) model.Stop = -1;
                model.GeneratedScript = _service.LRange(model.Key, model.Start.Value, model.Stop.Value);
                break;
            case "LSET":
                if (!model.Index.HasValue) model.Index = 0;
                model.GeneratedScript = _service.LSet(model.Key, model.Index.Value, model.Value);
                break;
        }

        return View(model);
    }
}

