using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Services;
using SharedLibrary.DTOs.User;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PoLoAnalysisMVC.Controllers;

public class LoginController : Controller
{
    // GET
    public IActionResult Index()
    {
        
        var token = Request.Cookies["session"];

        if (token is not null)
            return RedirectToAction("Index", "Home");
        
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Index(CatsUserLogin loginDto)
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
        var sessionCookie = CatsUserServices.GetTokenInfo(result);
        var sessionCookieOptions = new CookieOptions()
        {
            SameSite = SameSiteMode.None,
            Expires = sessionCookie.AccessTokenExpiration,
            Secure = true,
            HttpOnly = true,
            
        };
        
        Response.Cookies.Append("session",  sessionCookie.AccessToken,sessionCookieOptions);
        return RedirectToAction("Index", "Home");
    }
}