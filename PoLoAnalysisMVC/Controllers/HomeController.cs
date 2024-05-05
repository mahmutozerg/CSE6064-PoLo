using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Models;
using PoLoAnalysisMVC.Services;
using SharedLibrary;

namespace PoLoAnalysisMVC.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(AuthenticationSchemes = ApiConstants.SessionCookieName)]  
[ResponseCache(NoStore =true, Location =ResponseCacheLocation.None)]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    public  IActionResult Index()
    {

        return View();
    }
    

    public async Task<IActionResult> Import()
    {
        var userToken = Request.Cookies[ApiConstants.SessionCookieName];
        var user = await UserCourseServices.GetUserCourses(userToken);

        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}