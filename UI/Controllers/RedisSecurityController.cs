using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisSecurityController : Controller
{
    private readonly IRedisSecurityService _service;

    public RedisSecurityController(IRedisSecurityService service)
    {
        _service = service;
    }

    // RequirePass
    public IActionResult RequirePass() => View(new RedisRequirePassViewModel());

    [HttpPost]
    public IActionResult RequirePass(RedisRequirePassViewModel model)
    {
        model.GeneratedScript = _service.RequirePass(model.Password);
        return View(model);
    }

    // ACL SetUser
    public IActionResult ACLSetUser() => View(new RedisACLUserViewModel());

    [HttpPost]
    public IActionResult ACLSetUser(RedisACLUserViewModel model)
    {
        model.GeneratedScript = _service.ACLSetUser(model.UserName, model.Password, model.Permissions);
        return View(model);
    }

    // ACL DelUser
    public IActionResult ACLDelUser() => View(new RedisACLDelUserViewModel());

    [HttpPost]
    public IActionResult ACLDelUser(RedisACLDelUserViewModel model)
    {
        model.GeneratedScript = _service.ACLDelUser(model.UserName);
        return View(model);
    }

    // ACL List
    public IActionResult ACLList()
    {
        var model = new RedisACLListViewModel
        {
            GeneratedScript = _service.ACLList()
        };
        return View(model);
    }

    // ResetPass
    public IActionResult ResetPass() => View(new RedisResetPassViewModel());

    [HttpPost]
    public IActionResult ResetPass(RedisResetPassViewModel model)
    {
        model.GeneratedScript = _service.ResetPass(model.UserName, model.NewPassword);
        return View(model);
    }
}

