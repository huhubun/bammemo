using Microsoft.AspNetCore.Mvc;

namespace Bammemo.WebApi.Controllers;

public abstract class BammemoControllerBase : Controller
{
    internal CreatedResult Created(string actionName, string controllerName, string id, object value) =>
        Created(Url.Link($"{controllerName}_{actionName}", new { id }), value);
}
