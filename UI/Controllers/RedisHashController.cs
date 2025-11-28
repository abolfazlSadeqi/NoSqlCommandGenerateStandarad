using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisHashController : Controller
{
    private readonly IRedisHashService _service;

    public RedisHashController(IRedisHashService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisHashOperationViewModel());

    [HttpPost]
    public IActionResult Index(RedisHashOperationViewModel model, string actionType)
    {
        switch (actionType)
        {
            case "HSET":
                model.GeneratedScript = _service.HSet(model.Key, model.Field, model.Value);
                break;
            case "HGET":
                model.GeneratedScript = _service.HGet(model.Key, model.Field);
                break;
            case "HGETALL":
                model.GeneratedScript = _service.HGetAll(model.Key);
                break;
            case "HDEL":
                model.GeneratedScript = _service.HDel(model.Key, model.Field);
                break;
        }

        return View(model);
    }
}

