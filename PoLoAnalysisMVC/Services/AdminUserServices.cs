using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using SharedLibrary.DTOs.User;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Services;

public static class AdminUserServices
{
    private const string GetActiveUsersWithCourseByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUsersWithCourseByPage";

    private const string GetUsersWithCourseByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetAllUsersWithCourseByPage";

    private const string GetActiveUsersByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUsersByPage";

    private const string GetUsersWithPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetAllUsersByPage";

    private const string GetUserWithCoursesByEmailByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetUserWithCoursesByEmailByPage";
    
    private const string GetActiveUserWithCoursesByEmailUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUserWithcourses";
    
    private const string GetUserByEmailByPageUrl =
        ApiConstants.BusinessApiIp +"/api/AdminUser/GetUser";
    
    private const string GetActiveUserByEMailByPageUrl =
        ApiConstants.BusinessApiIp +"/api/AdminUser/GetActiveUser";

    private const string GetUserWithCoursesByIdUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetUserWithCoursesById";

    private const string AddUserToCoursesUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/AddUserToCourses";

    private const string RemoveUserFromCoursesUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/RemoveUserFromCourse";
    public static async Task<List<AppUser>?> GetUserWithFilters(string search, bool withCourses, bool getAll ,string page,string token)
    {
        return withCourses switch
        {
            true when (getAll && !string.IsNullOrEmpty(search)) =>await GetUsersWithSearchAsync(search, GetUserWithCoursesByEmailByPageUrl, token,page),
            true when (!getAll && !string.IsNullOrEmpty(search)) =>await GetUsersWithSearchAsync(search, GetActiveUserWithCoursesByEmailUrl, token,page),
            
            false when (getAll && !string.IsNullOrEmpty(search)) => await GetUsersWithSearchAsync(search,GetUserByEmailByPageUrl,token,page),
            false when (!getAll && !string.IsNullOrEmpty(search)) => await GetUsersWithSearchAsync(search,GetActiveUserByEMailByPageUrl,token,page),
            
            true when (getAll) => await GetUsersAsync(GetUsersWithCourseByPageUrl, page, token),
            true when !getAll => await GetUsersAsync(GetActiveUsersWithCourseByPageUrl, page, token),
            
            false when getAll => await GetUsersAsync(GetUsersWithPageUrl, page, token),
            false when !getAll => await GetUsersAsync(GetActiveUsersByPageUrl, page, token),
            
            
            
            _ => await GetUsersAsync(GetUsersWithPageUrl, page, token)
        };
    }

    private static async Task<List<AppUser>?> GetUsersAsync(string url,string page,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(url+$"?page={page}");
        
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<List<AppUser>>();
    }
    
    private static async Task<List<AppUser>?> GetUsersWithSearchAsync(string search,string url,string token,string page)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(url+$"?eMail={search}&page={page}");
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<List<AppUser>>();
    }
    
    public static async Task<AppUser?> GetUserWithCoursesByIdAsync(string id,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(GetUserWithCoursesByIdUrl+$"?id={id}");
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<AppUser>();
    }


    public static async Task<bool> AddUserToCourseAsync(List<string> courses, string eMail, string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);

        var dto = new AddUsersToCoursesDto()
        {
            CoursesFullNames = courses,
            TeacherEmail = eMail
        };
        
        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PutAsync(AddUserToCoursesUrl,content);

        return response.IsSuccessStatusCode;
    }
    
    public static async Task<bool> RemoveUserFromCoursesAsync(List<string> courses, string eMail, string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);

        var dto = new RemoveUserFromCourseDto()
        {
            CoursesFullNames = courses,
            TeacherEmail = eMail
        };
        
        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(RemoveUserFromCoursesUrl,content);

        return response.IsSuccessStatusCode;
    }

}