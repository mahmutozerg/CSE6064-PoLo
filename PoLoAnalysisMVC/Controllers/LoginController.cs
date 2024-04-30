using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Services;
using SharedLibrary.DTOs.User;
using System.Web;

namespace PoLoAnalysisMVC.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> SignIn(CatsUserLogin loginDto)
    {

        var token = Request.Cookies["session"];

        if (token is not null)
            return RedirectToAction("Index", "Home");

        var result = await CatsUserServices.LoginUser(loginDto);

        if (!result.HasValues)
        {

            ModelState.AddModelError("LoginError", "Please Check your credentials");
            return View("Index", loginDto);

        }
        //var cookies = CatsUserServices.AddCookies(result);
        //foreach (var cookie in cookies)
        //{
        //    Response.Cookies.Add(cookie);
        //}

        return RedirectToAction("Index", "Home");
    }
}