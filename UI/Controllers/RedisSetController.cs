using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisSetController : Controller
{
    private readonly IRedisSetService _service;

    public RedisSetController(IRedisSetService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisSetOperationViewModel());

    [HttpPost]
    public IActionResult Index(RedisSetOperationViewModel model, string actionType)
    {
        switch (actionType)
        {
            case "SADD":
                model.GeneratedScript = _service.SAdd(model.Key, model.Value);
                break;
            case "SREM":
                model.GeneratedScript = _service.SRem(model.Key, model.Value);
                break;
            case "SMEMBERS":
                model.GeneratedScript = _service.SMembers(model.Key);
                break;
            case "SCARD":
                model.GeneratedScript = _service.SCard(model.Key);
                break;
            case "ZADD":
                if (!model.Score.HasValue) model.Score = 0;
                model.GeneratedScript = _service.ZAdd(model.Key, model.Score.Value, model.Value);
                break;
            case "ZREM":
                model.GeneratedScript = _service.ZRem(model.Key, model.Value);
                break;
            case "ZRANGE":
                if (!model.Start.HasValue) model.Start = 0;
                if (!model.Stop.HasValue) model.Stop = -1;
                model.GeneratedScript = _service.ZRange(model.Key, model.Start.Value, model.Stop.Value);
                break;
            case "ZRANGEBYSCORE":
                if (!model.MinScore.HasValue) model.MinScore = 0;
                if (!model.MaxScore.HasValue) model.MaxScore = 100;
                model.GeneratedScript = _service.ZRangeByScore(model.Key, model.MinScore.Value, model.MaxScore.Value);
                break;
        }

        return View(model);
    }
}
