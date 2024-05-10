﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.Services;
using SharedLibrary.DTOs.User;
using SharedLibrary;
using SharedLibrary.DTOs.Tokens;

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

        var token = Request.Cookies[ApiConstants.SessionCookieName];

        if (token is not null)
            return RedirectToAction("Index", "Home");

        var tokenDto = await CatsUserServices.LoginUser(loginDto);


        if (string.IsNullOrEmpty(tokenDto.AccessToken))
        {
            ModelState.AddModelError("LoginError", "Please Check your credentials");
            return View("Index", loginDto);

        }

        var sessionCookieOptions = new CookieOptions()
        {
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(tokenDto.AccessTokenExpiration),
            Secure = true,
            
        };
        var refreshCookieOptions = new CookieOptions()
        {
            SameSite = SameSiteMode.Strict,
            Expires = new DateTimeOffset(tokenDto.RefreshTokenExpiration),
            Secure = true,
        };
        Response.Cookies.Append(ApiConstants.SessionCookieName,  tokenDto.AccessToken,sessionCookieOptions);
        Response.Cookies.Append(ApiConstants.RefreshCookieName,  tokenDto.RefreshToken,refreshCookieOptions);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<ActionResult> RefreshToken(string refreshToken)
    {
        var tokenDto =await CatsUserServices.CreateTokenByRefreshToken(refreshToken);

        if (tokenDto is null)
           return RedirectToAction("LogOut", "LogOut");


        return Json(new {
            AccessToken = tokenDto.AccessToken,
            AccessTokenExpiration = tokenDto.AccessTokenExpiration,
            RefreshToken = tokenDto.RefreshToken,
            RefreshTokenExpiration = tokenDto.RefreshTokenExpiration
        });        
    }

}