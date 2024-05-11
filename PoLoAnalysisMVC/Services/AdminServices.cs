using Newtonsoft.Json.Linq;
using SharedLibrary;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Services;

public static class AdminServices
{
    private const string GetActiveUsersWithCourseByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUsersWithCourseByPage";

    private const string GetUsersWithCourseByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetAllUsersWithCourseByPage";

    private const string GetActiveUsersByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUsersByPage";

    private const string GetUsersWithPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetAllUsersByPage";

    private const string GetUserWithCoursesByEmailUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetUserWithCourses";
    
    private const string GetActiveUserWithCoursesByEmailUrl =
        ApiConstants.BusinessApiIp + "/api/AdminUser/GetActiveUserWithcourses";
    
    private const string GetUserUrl =
        ApiConstants.BusinessApiIp +"/api/AdminUser/GetUser";
    
    private const string GetActiveUserUrl =
        ApiConstants.BusinessApiIp +"/api/AdminUser/GetActiveUser";
    public static async Task<List<AppUser>?> GetUserWithFilters(string search, bool withCourses, bool getAll ,string page,string token)
    {
        return withCourses switch
        {
            true when (getAll && !string.IsNullOrEmpty(search)) =>await GetUsersWithSearchAsync(search, GetUserWithCoursesByEmailUrl, token),
            true when (!getAll && !string.IsNullOrEmpty(search)) =>await GetUsersWithSearchAsync(search, GetActiveUserWithCoursesByEmailUrl, token),
            
            false when (getAll && !string.IsNullOrEmpty(search)) => await GetUsersWithSearchAsync(search,GetUserUrl,token),
            false when (!getAll && !string.IsNullOrEmpty(search)) => await GetUsersWithSearchAsync(search,GetActiveUserUrl,token),
            
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
    
    private static async Task<List<AppUser>?> GetUsersWithSearchAsync(string search,string url,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(url+$"?eMail={search}");
        
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<List<AppUser>>();
    }
}