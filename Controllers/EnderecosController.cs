using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace projetoAeC.Controllers;

[Authorize]
public class EnderecosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
