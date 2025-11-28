using Core;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace UI.Controllers;

public class RedisKeyController : Controller
{
    private readonly IRedisKeyScriptService _service;
    private readonly IRedisKeyService _servicekey;

    public RedisKeyController(IRedisKeyScriptService service, IRedisKeyService servicekey)
    {
        _service = service;
        _servicekey = servicekey;

    }

    public IActionResult AddKey() => View(new RedisKeyAddViewModel());

    [HttpPost]
    public IActionResult AddKey(RedisKeyAddViewModel model)
    {
        model.GeneratedScript = _service.AddKey(model.DatabaseNumber, model.Key, model.Value, model.TTLSeconds);
        return View(model);
    }


    public IActionResult UpdateKey() => View(new RedisKeyUpdateViewModel());

    [HttpPost]
    public IActionResult UpdateKey(RedisKeyUpdateViewModel model)
    {
        model.GeneratedScript = _service.UpdateKey(model.DatabaseNumber, model.Key, model.NewValue);
        return View(model);
    }


    public IActionResult DeleteKey() => View(new RedisKeyDeleteViewModel());

    [HttpPost]
    public IActionResult DeleteKey(RedisKeyDeleteViewModel model)
    {
        model.GeneratedScript = _service.DeleteKey(model.DatabaseNumber, model.Key);
        return View(model);
    }


    public IActionResult TTL() => View(new RedisKeyTTLViewModel());

    [HttpPost]
    public IActionResult TTL(RedisKeyTTLViewModel model)
    {
        model.GeneratedScript = _service.SetTTL(model.DatabaseNumber, model.Key, model.TTLSeconds);
        return View(model);
    }


    public IActionResult RenameKey() => View(new RedisKeyRenameViewModel());

    [HttpPost]
    public IActionResult RenameKey(RedisKeyRenameViewModel model)
    {
        model.GeneratedScript = _service.RenameKey(model.DatabaseNumber, model.OldKey, model.NewKey);
        return View(model);
    }


    // Persist
    public IActionResult Persist() => View(new RedisKeyPersistViewModel());

    [HttpPost]
    public IActionResult Persist(RedisKeyPersistViewModel model)
    {
        model.GeneratedScript = _service.PersistKey(model.DatabaseNumber, model.Key);
        return View(model);
    }

    // Copy Key
    public IActionResult Copy() => View(new RedisKeyCopyViewModel());

    [HttpPost]
    public IActionResult Copy(RedisKeyCopyViewModel model)
    {
        model.GeneratedScript = _service.CopyKey(model.SourceDatabase, model.TargetDatabase, model.Key);
        return View(model);
    }

    // Move Key
    public IActionResult Move() => View(new RedisKeyMoveViewModel());

    [HttpPost]
    public IActionResult Move(RedisKeyMoveViewModel model)
    {
        model.GeneratedScript = _service.MoveKey(model.SourceDatabase, model.TargetDatabase, model.Key);
        return View(model);
    }
   
       

        public IActionResult RedisKeyOperation() => View(new RedisKeyOperationViewModel());

        [HttpPost]
        public IActionResult RedisKeyOperation(RedisKeyOperationViewModel model, string actionType)
        {
            switch (actionType)
            {
                case "SET":
                    model.GeneratedScript = _servicekey.Set(model.Key, model.Value, model.ExpireSeconds);
                    break;
                case "GET":
                    model.GeneratedScript = _servicekey.Get(model.Key);
                    break;
                case "DEL":
                    model.GeneratedScript = _servicekey.Del(model.Key);
                    break;
                case "EXISTS":
                    model.GeneratedScript = _servicekey.Exists(model.Key);
                    break;
                case "RENAME":
                    model.GeneratedScript = _servicekey.Rename(model.Key, model.NewKey);
                    break;
                case "TTL":
                    model.GeneratedScript = _servicekey.TTL(model.Key);
                    break;
                case "INCR":
                    model.GeneratedScript = _servicekey.Incr(model.Key);
                    break;
                case "APPEND":
                    model.GeneratedScript = _servicekey.Append(model.Key, model.Value);
                    break;
            }

            return View(model);
        
    }

}

