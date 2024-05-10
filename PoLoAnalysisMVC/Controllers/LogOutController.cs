using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace PoLoAnalysisMVC.Controllers;

public class LogOutController : Controller
{
    // GET
    [Route("logout")]
    public IActionResult LogOut()
    {

        if (Request.Cookies.ContainsKey(ApiConstants.SessionCookieName))
        {
            Response.Cookies.Delete(ApiConstants.SessionCookieName); // Remove the "Session" cookie
        }

        if (Request.Cookies.ContainsKey((ApiConstants.RefreshCookieName)))
        {
            Response.Cookies.Delete(ApiConstants.RefreshCookieName); // Remove the "Session" cookie

        }

        return RedirectToAction("Index", "Login");
    }
}