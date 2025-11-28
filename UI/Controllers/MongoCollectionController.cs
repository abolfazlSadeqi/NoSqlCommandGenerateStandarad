using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;

public class MongoCollectionController : Controller
{
    private readonly IMongoCollectionScriptService _service;

    public MongoCollectionController(IMongoCollectionScriptService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult CreateCollection()
    {
        return View(new MongoCreateCollectionViewModel());
    }

    [HttpPost]
    public IActionResult CreateCollection(MongoCreateCollectionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateCreateCollectionScript(model);
        return View(model);
    }
    [HttpGet]
    public IActionResult DropCollection()
    {
        return View(new MongoDropCollectionViewModel());
    }

    [HttpPost]
    public IActionResult DropCollection(MongoDropCollectionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateDropCollectionScript(model);
        return View(model);
    }

    [HttpGet]
    public IActionResult ListCollections()
    {
        return View(new MongoListCollectionsViewModel());
    }

    [HttpPost]
    public IActionResult ListCollections(MongoListCollectionsViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateShowCollectionsScript(model);

        // Optional: simulate results for preview
        if (!string.IsNullOrWhiteSpace(model.SearchKeyword))
            model.Collections = new string[] { "users", "orders", "products" }
                .Where(c => c.Contains(model.SearchKeyword))
                .ToArray();
        else
            model.Collections = new string[] { "users", "orders", "products", "logs", "sessions" };

        return View(model);
    }
    [HttpGet]
    public IActionResult RenameCollection()
    {
        return View(new MongoRenameCollectionViewModel());
    }

    [HttpPost]
    public IActionResult RenameCollection(MongoRenameCollectionViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        model.GeneratedScript = _service.GenerateRenameCollectionScript(model);
        return View(model);
    }

}

