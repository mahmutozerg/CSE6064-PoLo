using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Models;

namespace PoLoAnalysisMVC.Controllers;

public class ErrorController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View("Error",new ErrorViewModel()
        {
            RequestId = HttpContext.Request.ToString()
        });
    }
}