using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

namespace UI.Controllers;


public class MongoDocumentController : Controller
{
    private readonly IMongoDocumentService _service;

    public MongoDocumentController(IMongoDocumentService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Insert()
    {
        var model = new MongoDocumentInsertViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Insert(MongoDocumentInsertViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        model.GeneratedScript = _service.GenerateInsertScript(model);
        return View(model);
    }

    [HttpGet]
    public IActionResult Update()
    {
        var model = new MongoDocumentUpdateViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(MongoDocumentUpdateViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var service = new MongoDocumentService();
        model.GeneratedScript = service.GenerateUpdateScript(model);

        return View(model);
    }
    [HttpGet]
    public IActionResult Delete()
    {
        return View(new MongoDocumentDeleteViewModel());
    }

    [HttpPost]
    public IActionResult Delete(MongoDocumentDeleteViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var service = new MongoDocumentService();
        model.GeneratedScript = service.GenerateDeleteScript(model);

        return View(model);
    }

}
