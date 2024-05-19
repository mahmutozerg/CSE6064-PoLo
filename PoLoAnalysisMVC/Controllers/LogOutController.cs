using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary;

namespace PoLoAnalysisMVC.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(AuthenticationSchemes = ApiConstants.SessionCookieName)]  
[ResponseCache(NoStore =true, Location =ResponseCacheLocation.None)]
public class LogOutController : Controller
{
    // GET
    [Route("logout")]
    public IActionResult LogOut()
    {

        if (Request.Cookies.ContainsKey(ApiConstants.SessionCookieName))
            Response.Cookies.Delete(ApiConstants.SessionCookieName); // Remove the "Session" cookie

        if (Request.Cookies.ContainsKey((ApiConstants.RefreshCookieName)))
            Response.Cookies.Delete(ApiConstants.RefreshCookieName); // Remove the "Session" cookie

        

        return RedirectToAction("Index", "Login");
    }
}