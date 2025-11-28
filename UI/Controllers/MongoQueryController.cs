using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class MongoQueryController : Controller
{
    private readonly IMongoQueryService _service;

    public MongoQueryController(IMongoQueryService service)
    {
        _service = service;
    }


    [HttpGet]
    public IActionResult Query()
    {
        var vm = new MongoQueryViewModel();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Query(MongoQueryViewModel vm)
    {
        vm.GeneratedScript = _service.GenerateQueryScript(vm);
        return View(vm);
    }
}

