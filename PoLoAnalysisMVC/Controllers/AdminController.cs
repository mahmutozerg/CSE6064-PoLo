using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Services;
using SharedLibrary;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[Authorize(AuthenticationSchemes = ApiConstants.SessionCookieName ,Roles = "Admin")]  
public class AdminController : Controller
{
    
    [HttpGet]
    public async Task<IActionResult> Users(string search, bool withCourses = false, bool getAll = false,string page = "0")
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var users = await AdminServices.GetUserWithFilters(search,withCourses,getAll,page,token) ?? new List<AppUser>();
        ViewData["getAll"] = getAll;
        ViewData["withCourses"] = withCourses;
        ViewData["page"] = page;
        ViewData["search"] = search;
        return View(users);
    }
    

}
