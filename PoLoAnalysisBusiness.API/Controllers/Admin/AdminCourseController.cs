using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.DTOS.Courses;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.DTO.Courses;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusinessAPI.Controllers.Admin;
[Authorize(Roles = "Admin")]
public class AdminCourseController:CustomControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<AdminCourseController> _logger;

    public AdminCourseController(ICourseService courseService, ILogger<AdminCourseController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> GetActiveCoursesByPageByName(string name,string page)
    {
    
        return CreateActionResult(await _courseService.GetActiveCoursesByNameByPageAsync(name,page));
    }
    [HttpGet]
    public async Task<IActionResult> GetActiveCoursesByPage(string page)
    {   
    
        return CreateActionResult(await _courseService.GetActiveCoursesByPageAsync(page));
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCoursesByPageByName(string name ,string page)
    {
        return CreateActionResult(await _courseService.GetAllCoursesByPageByNameAsync(name,page));
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllCoursesByPage(string page)
    {
        return CreateActionResult(await _courseService.GetAllCoursesByPageAsync(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCompulsoryCoursesByPage(string page)
    {
        return CreateActionResult(await _courseService.GetAllCompulsoryCoursesByPage(page));
    }
    [HttpGet]
    public async Task<IActionResult> GetActiveCompulsoryCoursesByPage(string page)
    {
        return CreateActionResult(await _courseService.GetActiveCompulsoryCoursesByPage(page));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCompulsoryCoursesByPageByName(string name,string page)
    {
        return CreateActionResult(await _courseService.GetAllCompulsoryCoursesByPageByName(name,page));
    }
    [HttpGet]
    public async Task<IActionResult> GetActiveCompulsoryCoursesByPageByName(string name,string page)
    {
        return CreateActionResult(await _courseService.GetActiveCompulsoryCoursesByPageByName(name,page));
    }
    
    [HttpPut]
    public async Task<IActionResult> AddCourse([FromBody]CourseAddDto courseAddDto)
    {
        var course = new Course()
        {
            IsCompulsory = courseAddDto.IsCompulsory,
            Id = courseAddDto.CourseCode +" "+ courseAddDto.CourseYear,
            Year = courseAddDto.CourseYear
        };
        var createdBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var res =await _courseService.AddAsync(course,createdBy);

        return CreateActionResult(res);
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(string id)
    {
        var updatedBy = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var res =await _courseService.DeleteCourseWithFilesWithResultByIdAsync(id, updatedBy);

        return CreateActionResult(res);
    }
    

}