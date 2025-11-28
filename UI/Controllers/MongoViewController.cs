using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class MongoViewController : Controller
{
    private readonly IMongoViewService _service;

    public MongoViewController(IMongoViewService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult ListView()
    {
        return View(new MongoViewViewModel());
    }

    [HttpPost]
    public IActionResult ListView(MongoViewViewModel vm)
    {
        if (vm.OperationType != "list")
            vm.GeneratedScript = _service.GenerateScript(vm);

        return View(vm);
    }
  

}
