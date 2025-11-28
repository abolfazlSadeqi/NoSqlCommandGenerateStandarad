using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisCounterController : Controller
{
    private readonly IRedisCounterService _service;

    public RedisCounterController(IRedisCounterService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisCounterViewModel());

    [HttpPost]
    public IActionResult Index(RedisCounterViewModel model, string actionType)
    {
        long amount = model.Amount ?? 1;

        switch (actionType)
        {
            case "INCR":
                model.GeneratedScript = _service.Incr(model.Key, amount);
                break;
            case "DECR":
                model.GeneratedScript = _service.Decr(model.Key, amount);
                break;
        }

        return View(model);
    }
}

