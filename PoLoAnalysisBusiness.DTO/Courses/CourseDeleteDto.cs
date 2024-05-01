using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisBusiness.DTO.Courses;

public class CourseDeleteDto
{
    [Required(ErrorMessage = "Coursecode field is required")]
    public string CourseCode { get; set; }
    
    [Required(ErrorMessage = "CourseYear field is required")]
    public string CourseYear { get; set; }

}