﻿using Newtonsoft.Json.Linq;
using SharedLibrary;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Services;

public static class UserCourseServices
{
    private const string GetUserCourseUrl = ApiConstants.BusinessApiIp + "/api/User/GetUserCourses";


    public static async Task<AppUser> GetUserCourses(string token)
    {
        
        using var client = new HttpClient();
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        
        var response = await client.GetAsync(GetUserCourseUrl);

        if (!response.IsSuccessStatusCode)
            return new AppUser();
        
        var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
        return jsonResult["data"].ToObject<AppUser>();

    }
}