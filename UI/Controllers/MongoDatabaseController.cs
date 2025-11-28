using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class MongoDatabaseController : Controller
{
    private readonly IMongoScriptService _service;

    public MongoDatabaseController(IMongoScriptService service)
    {
        _service = service;
    }

    public IActionResult CreateDatabase()
    {
        return View(new MongoDatabaseCreateViewModel());
    }

    [HttpPost]
    public IActionResult CreateDatabase(MongoDatabaseCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateCreateDatabaseScript(model);

        return View(model);
    }

    [HttpGet]
    public IActionResult DropDatabase()
    {
        return View(new MongoDropDatabaseViewModel());
    }

    [HttpPost]
    public IActionResult DropDatabase(MongoDropDatabaseViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateDropDatabaseScript(
            model.DatabaseName,
            model.ForceDelete
        );

        return View(model);
    }


    [HttpGet]
    public IActionResult ListDatabases()
    {
        return View(new MongoListDatabasesViewModel());
    }

    [HttpPost]
    public IActionResult ListDatabases(MongoListDatabasesViewModel model)
    {
        model.GeneratedScript = _service.GenerateListDatabasesScript();
        return View(model);
    }
    [HttpGet]
    public IActionResult UseDatabase()
    {
        return View(new MongoUseDatabaseViewModel());
    }

    [HttpPost]
    public IActionResult UseDatabase(MongoUseDatabaseViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateUseDatabaseScript(model.DatabaseName);
        return View(model);
    }
  
}

