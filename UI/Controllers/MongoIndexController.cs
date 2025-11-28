using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class MongoIndexController : Controller
{
    private readonly IMongoIndexScriptService _service;

    public MongoIndexController(IMongoIndexScriptService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult CreateIndex()
    {
        return View(new MongoCreateIndexViewModel());
    }

    [HttpPost]
    public IActionResult CreateIndex(MongoCreateIndexViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateCreateIndexScript(model);
        return View(model);
    }

    [HttpGet]
    public IActionResult DropIndex()
    {
        return View(new MongoDropIndexViewModel());
    }

    [HttpPost]
    public IActionResult DropIndex(MongoDropIndexViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateDropIndexScript(model);
        return View(model);
    }

    [HttpGet]
    public IActionResult GenerateIndexes()
    {
        var model = new MongoIndexViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult GenerateIndexes(MongoIndexViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        model.Indexes = _service.MockIndexes(model.CollectionName);
        model.GeneratedScript = _service.GenerateListIndexesScript(model.DatabaseName, model.CollectionName);

        return View(model);
    }
    [HttpGet]
    public IActionResult AdvanceIndexGenerateScript()
    {
        var vm = new AdvancedIndexViewModel();
        vm.Fields.Add(new IndexAdvancedField());
        return View(vm);
    }

    [HttpPost]
    public IActionResult AdvanceIndexGenerateScript(AdvancedIndexViewModel vm)
    {
        vm.GeneratedScript = _service.AdvanceIndexGenerateScript(vm);
        return View(vm);
    }

}

