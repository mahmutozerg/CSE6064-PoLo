using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Models;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.DTO.Courses;
using SharedLibrary.DTOs.Responses;
using StatusCodes = SharedLibrary.StatusCodes;

namespace PoLoAnalysisBusinessAPI.Controllers.Admin;
[Authorize(Roles = "Admin")]

public class AdminCourseController:CustomControllerBase
{
    private readonly ICourseService _courseService;

    public AdminCourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }


    [HttpGet]
    public async Task<IActionResult> GetActiveCourses()
    {
        var courses = await _courseService.Where(c => !c.IsDeleted).ToListAsync();
        return CreateActionResult(CustomResponseListDataDto<Course>.Success(courses, StatusCodes.Ok));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.Where(c=> c!=null).ToListAsync();
        return CreateActionResult(CustomResponseListDataDto<Course>.Success(courses, StatusCodes.Ok));
    }

    [HttpPost]
    public async Task<IActionResult> AddCourse(CourseAddDto courseAddDto)
    {
        var course = new Course()
        {
            IsCompulsory = courseAddDto.IsCompulsory,
            Id = courseAddDto.CourseCode + courseAddDto.CourseYear,
            Year = courseAddDto.CourseYear
        };
        var createdBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var res =await _courseService.AddAsync(course,createdBy);

        return CreateActionResult(res);
    }
    
    
    [HttpDelete]
    public async Task<IActionResult> DeleteCourse(CourseDeleteDto courseDeleteDto)
    {
        var courseId = courseDeleteDto.CourseCode + courseDeleteDto.CourseYear;
        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var res =await _courseService.Remove(courseId, updatedBy);

        return CreateActionResult(res);
    }
    

}