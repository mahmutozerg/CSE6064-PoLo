using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisBusiness.DTO.Courses;

public class CourseUpdateDto
{
    [Required(ErrorMessage = "CurrentCourseCode field is required")]
    public string CurrentCourseCode{ get; set; }
    
    
    [Required(ErrorMessage = "CurrentCourseYear field is required")]
    public string CurrentCourseYear{ get; set; }

    public string? UpdatedCourseCode { get; set; }
    public string? UpdatedCourseYear { get; set; }
    public bool? UpdatedCourseIsCompulsory { get; set; }
    
}