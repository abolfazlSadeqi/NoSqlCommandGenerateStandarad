using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class MongoSecurityController : Controller
{
    private readonly IMongoSecurityService _service;

    public MongoSecurityController(IMongoSecurityService service)
    {
        _service = service;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var model = new MongoAdminViewModel
        {
            UserModel = new MongoUserManagementViewModel(),
            RoleModel = new MongoRoleManagementViewModel()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult GenerateUserScript(MongoUserManagementViewModel userModel)
    {
        if (!ModelState.IsValid) return View("Index", new MongoAdminViewModel { UserModel = userModel, RoleModel = new MongoRoleManagementViewModel() });

        userModel.GeneratedScript = _service.GenerateUserScript(userModel);
        return View("Index", new MongoAdminViewModel { UserModel = userModel, RoleModel = new MongoRoleManagementViewModel() });
    }

    [HttpPost]
    public IActionResult GenerateRoleScript(MongoRoleManagementViewModel roleModel)
    {
        if (!ModelState.IsValid) return View("Index", new MongoAdminViewModel { RoleModel = roleModel, UserModel = new MongoUserManagementViewModel() });

        roleModel.GeneratedScript = _service.GenerateRoleScript(roleModel);
        return View("Index", new MongoAdminViewModel { RoleModel = roleModel, UserModel = new MongoUserManagementViewModel() });
    }


}
