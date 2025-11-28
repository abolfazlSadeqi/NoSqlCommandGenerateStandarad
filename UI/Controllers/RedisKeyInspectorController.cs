using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisKeyInspectorController : Controller
{
    private readonly IRedisKeyInspectorService _service;

    public RedisKeyInspectorController(IRedisKeyInspectorService service)
    {
        _service = service;
    }

    public IActionResult Index() => View(new RedisKeyInspectorViewModel());

    [HttpPost]
    public IActionResult Index(RedisKeyInspectorViewModel model)
    {
        model.GeneratedScript = _service.InspectKey(model);
        return View(model);
    }
}

