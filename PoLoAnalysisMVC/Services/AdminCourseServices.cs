using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Services;

public static class AdminCourseServices
{
    private const string GetActiveCoursesByPageByNameUrl =
        ApiConstants.BusinessApiIp +"/api/AdminCourse/GetActiveCoursesByPageByName";

    private const string GetActiveCoursesByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminCourse/GetActiveCoursesByPage";

    private const string GetAllCoursesByPageUrl =
        ApiConstants.BusinessApiIp + "/api/AdminCourse/GetAllCoursesByPage";

    private const string GetAllCoursesByPageByNameUrl =
        ApiConstants.BusinessApiIp + "/api/AdminCourse/GetAllCoursesByPageByName";

    private const string AddCourseUrl =
        ApiConstants.BusinessApiIp + "/api/AdminCourse/AddCourse";

    private const string DeleteCourseUrl =
        ApiConstants.BusinessApiIp + "/api/AdminCourse/DeleteCourse";

    private const string GetAllCompulsoryCoursesByPage
        = ApiConstants.BusinessApiIp + "/api/AdminCourse/GetAllCompulsoryCoursesByPage";
        
        
    private const string GetActiveCompulsoryCoursesByPage
        = ApiConstants.BusinessApiIp + "/api/AdminCourse/GetActiveCompulsoryCoursesByPage";
    
    private const string GetAllCompulsoryCoursesByPageByName
        = ApiConstants.BusinessApiIp + "/api/AdminCourse/GetAllCompulsoryCoursesByPageByName";

    private const string GetActiveCompulsoryCoursesByPageByName
        = ApiConstants.BusinessApiIp + "/api/AdminCourse/GetActiveCompulsoryCoursesByPageByName";


    public static async Task<List<Course>?> GetCoursesWithFilters(string search, bool isCompulsory, bool getAll ,string page,string token)
    {
        return isCompulsory switch
        {
            true when (getAll && !string.IsNullOrEmpty(search)) =>await GetCoursesWithSearchAsync(search, GetAllCompulsoryCoursesByPageByName, token,page),
            true when (!getAll && !string.IsNullOrEmpty(search)) =>await GetCoursesWithSearchAsync(search, GetActiveCompulsoryCoursesByPageByName, token,page),
            
            false when (getAll && !string.IsNullOrEmpty(search)) => await GetCoursesWithSearchAsync(search,GetAllCoursesByPageByNameUrl,token,page),
            false when (!getAll && !string.IsNullOrEmpty(search)) => await GetCoursesWithSearchAsync(search,GetActiveCoursesByPageByNameUrl,token,page),
            
            true when (getAll) => await GetCoursesAsync(GetAllCompulsoryCoursesByPage, page, token),
            true when !getAll => await GetCoursesAsync(GetActiveCompulsoryCoursesByPage, page, token),
            
            false when getAll => await GetCoursesAsync(GetAllCoursesByPageUrl, page, token),
            false when !getAll => await GetCoursesAsync(GetActiveCoursesByPageUrl, page, token),
            
            
            
            _ => await GetCoursesAsync(GetActiveCoursesByPageUrl, page, token)
        };
    }

    private static async Task<List<Course>?> GetCoursesAsync(string url,string page,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(url+$"?page={page}");
        
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<List<Course>>();
    }
    
    private static async Task<List<Course>?> GetCoursesWithSearchAsync(string search,string url,string token,string page)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.GetAsync(url+$"?name={search}&page={page}");
        
        return !response.IsSuccessStatusCode ? null : JObject.Parse(await response.Content.ReadAsStringAsync())["data"].ToObject<List<Course>>();
    }
    
    public static async Task<bool> DeleteCourseByIdAsync(string id,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.DeleteAsync(DeleteCourseUrl+$"/{id}");
        
        return response.IsSuccessStatusCode;
    }
    
    public static async Task<bool> AddCourseAsync(CourseAddDto dto,string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        
        var response = await client.PutAsync(AddCourseUrl,content);
        
        return response.IsSuccessStatusCode;
    }
}