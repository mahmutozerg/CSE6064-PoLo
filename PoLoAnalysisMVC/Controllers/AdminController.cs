using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisMVC.DTOS;
using PoLoAnalysisMVC.Services;
using SharedLibrary;
using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[Authorize(AuthenticationSchemes = ApiConstants.SessionCookieName)]  
public class AdminController : Controller
{
    
    [HttpGet]
    public async Task<IActionResult> Users(string search, bool withCourses = false, bool getAll = false,string page = "0")
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var users = await AdminUserServices.GetUserWithFilters(search,withCourses,getAll,page,token) ?? new ();
        ViewData["getAll"] = getAll;
        ViewData["withCourses"] = withCourses;
        ViewData["page"] = page;
        ViewData["search"] = search;
        return View(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Courses(string search, bool isCompulsory = false, bool getAll = false,string page = "0")
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var courses = await AdminCourseServices.GetCoursesWithFilters(search,isCompulsory,getAll,page,token) ?? new ();
        ViewData["getAll"] = getAll;
        ViewData["page"] = page;
        ViewData["search"] = search;
        ViewData["isCompulsory"] = isCompulsory;
        return View(courses);
    }
    
    [HttpGet]
    public IActionResult AddNewCourse()
    {
        return View(new CourseAddDto());

    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewCourse(CourseAddDto courseAddDto)
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var result = await AdminCourseServices.AddCourseAsync(courseAddDto, token);

        return result
            ? RedirectToAction("Courses", new { search = (courseAddDto.CourseCode +" "+ courseAddDto.CourseYear), isCompulsory=courseAddDto.IsCompulsory })
            : RedirectToAction("Index", "Error");

    }
    [HttpDelete]
    public async Task<IActionResult> DeleteCourse(string id)
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var response = await AdminCourseServices.DeleteCourseByIdAsync(id, token);

        return response ? Ok() : Problem();

    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateUsersCourse(string id)
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var user = await AdminUserServices.GetUserWithCoursesByIdAsync(id, token);
        if (user is null)
            return Redirect($"Error/Index/");
        
        var courses = new List<Course>();
        var page = 0;

        do
        {
            var currentCourse = await AdminCourseServices.GetCoursesWithFilters(string.Empty, false, false,page.ToString() , token);
            page += 1;
            if (currentCourse is not null && currentCourse.Count >0)
            {
                courses.AddRange(currentCourse);
                continue;
            }
            break;
            
        } while (true);
        
        foreach (var userCourse in user.Courses)
        {
            var existingCourse = courses.FirstOrDefault(c => c.Id == userCourse.Id);
            if (existingCourse != null)
            {
                courses.Remove(existingCourse);
            }
        }
  
        return View(new UserWithExistingCoursesDto()
        {
            AppUser = user,
            FreeCourses = courses,
            Courses = user.Courses
            
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateUsersCourse([FromBody] CourseUpdateDto model)
    {
        var token = Request.Cookies[ApiConstants.SessionCookieName];
        var user = await AdminUserServices.GetUserWithCoursesByIdAsync(model.Id,token);

        if (user is null)
            return Redirect($"Error/Index/");
        
        var coursesToBeAdded = model.SelectedCourses.Where(course => !user.Courses.Exists(c => c.Id == course)).ToList();
        var coursesToBeRemoved = model.FreeCourses.Where(course => user.Courses.Exists(c => c.Id == course)).ToList();

        var results = new List<bool>();
        if (coursesToBeAdded.Count > 0)
            results.Add(await AdminUserServices.AddUserToCourseAsync(coursesToBeAdded, user.EMail, token));
            
        results.Add( await AdminUserServices.RemoveUserFromCoursesAsync(coursesToBeRemoved, user.EMail, token));

       
        return  results.All(c => c) ? Ok() : Problem();
    }

 
    
}
