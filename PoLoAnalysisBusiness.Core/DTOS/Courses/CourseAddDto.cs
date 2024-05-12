using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisBusiness.DTO.Courses;

public class CourseAddDto
{
    [Required(ErrorMessage = "Coursecode field is required")]
    public string CourseCode { get; set; }
    
    [Required(ErrorMessage = "CourseYear field is required")]
    public string CourseYear { get; set; }
    [Required(ErrorMessage = "IsCompulsory field is required")]
    public bool IsCompulsory { get; set; }
}