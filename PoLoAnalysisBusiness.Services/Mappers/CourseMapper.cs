using PoLoAnalysisBusiness.DTO.Courses;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Services.Mappers;

public static class CourseMapper
{
    public static  void CourseUpdateMapper (ref Course course,CourseUpdateDto updateDto)
    {
        var courseCode= string.IsNullOrWhiteSpace(updateDto.UpdatedCourseCode) ? updateDto.CurrentCourseCode : updateDto.UpdatedCourseCode;
        var courseYear =string.IsNullOrWhiteSpace(updateDto.UpdatedCourseYear) ? updateDto.CurrentCourseYear : updateDto.UpdatedCourseYear;
        course.Id = courseCode + courseYear;
        course.IsCompulsory = updateDto.UpdatedCourseIsCompulsory ?? course.IsCompulsory;
        
    }    
}