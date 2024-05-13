using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisBusiness.Core.DTOS.Courses;

public class CourseDeleteDto
{
    [Required(ErrorMessage = "Coursecode field is required")]
    public string CourseId { get; set; }
    
 

}