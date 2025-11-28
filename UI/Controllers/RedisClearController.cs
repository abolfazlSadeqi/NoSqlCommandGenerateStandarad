using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisClearController : Controller
{
    private readonly IRedisScriptService _service;

    public RedisClearController(IRedisScriptService service)
    {
        _service = service;
    }

    public IActionResult ClearDatabase() => View(new RedisClearDatabaseViewModel());

    [HttpPost]
    public IActionResult ClearDatabase(RedisClearDatabaseViewModel model)
    {
     

        model.GeneratedScript = _service.GenerateClearDatabaseScript(model.DatabaseNumber);

        return View(model);
    }

    public IActionResult GenerateClearAllDatabaseScript() => View(new RedisClearAllDatabaseViewModel());

    [HttpPost]
    public IActionResult GenerateClearAllDatabaseScript(RedisClearAllDatabaseViewModel model)
    {


        model.GeneratedScript = _service.GenerateClearAllDatabaseScript();

        return View(model);
    }


    public IActionResult ClearPattern()
    => View(new RedisClearPatternViewModel());

    [HttpPost]
    public IActionResult ClearPattern(RedisClearPatternViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Pattern))
        {
            ModelState.AddModelError("", "Pattern is required.");
            return View(model);
        }

        model.GeneratedScript = _service.GeneratePatternDeleteScript(model.DatabaseNumber, model.Pattern);

        return View(model);
    }
    public IActionResult ClearExpired()
    => View(new RedisClearExpiredViewModel());

    [HttpPost]
    public IActionResult ClearExpired(RedisClearExpiredViewModel model)
    {
        model.GeneratedScript = _service.GenerateDeleteExpiredKeysScript(model.DatabaseNumber);
        return View(model);
    }

}
