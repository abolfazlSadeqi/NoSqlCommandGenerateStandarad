using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class RedisDatabaseController : Controller
{
  
    private readonly IRedisDatabaseService _service;

    public RedisDatabaseController(IRedisDatabaseService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult Manage()
    {
        return View(new RedisDatabaseViewModel());
    }

    [HttpPost]
    public IActionResult Manage(RedisDatabaseViewModel vm, string actionType)
    {
        switch (actionType)
        {
            case "select":
                vm.GeneratedScript = _service.GenerateSelectDatabase(vm.DatabaseNumber);
                break;

            case "clear":
                if (!vm.ConfirmDangerous)
                    ModelState.AddModelError("", "You must confirm the operation.");
                else
                    vm.GeneratedScript = _service.GenerateClearDatabase(vm.DatabaseNumber);
                break;

            case "export":
                vm.GeneratedScript = _service.GenerateExport(vm.DatabaseNumber, vm.ExportFormat);
                break;
        }

        return View(vm);
    }
}

