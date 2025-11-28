using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class MongoBulkController : Controller
{
    private readonly IMongoBulkService _service;

    public MongoBulkController(IMongoBulkService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult GenerateBulkScript()
    {
        return View(new MongoBulkViewModel());
    }

    [HttpPost]
    public IActionResult GenerateBulkScript(MongoBulkViewModel vm)
    {
        vm.GeneratedScript = _service.GenerateBulkScript(vm);
        return View(vm);
    }
}
