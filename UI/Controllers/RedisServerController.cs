using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisServerController : Controller
{
    private readonly IRedisServerCommandService _service;

    public RedisServerController(IRedisServerCommandService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisServerInfoViewModel());

    [HttpPost]
    public IActionResult Index(RedisServerInfoViewModel model)
    {
        model.GeneratedScript = _service.GenerateServerCommands(model);
        return View(model);
    }
}

