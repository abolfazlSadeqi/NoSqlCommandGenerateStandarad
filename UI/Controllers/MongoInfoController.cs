using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class MongoInfoController : Controller
{
    private readonly IMongoInfoService _service;

    public MongoInfoController(IMongoInfoService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult General()
    {
        return View(new MongoGeneralCommandViewModel());
    }

    [HttpPost]
    public IActionResult General(MongoGeneralCommandViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateGeneralCommandScript(model);
        return View(model);
    }


}

