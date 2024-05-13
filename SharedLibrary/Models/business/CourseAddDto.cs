using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models.business;

public class CourseAddDto
{
    [Required(ErrorMessage = "Coursecode field is required")]
    [StringLength(7, ErrorMessage = "The field must be a maximum of 3 characters.")]
    public string CourseCode { get; set; }
    
    [Required(ErrorMessage = "CourseYear field is required")]
    public string CourseYear { get; set; }
    [Required(ErrorMessage = "IsCompulsory field is required")]
    public bool IsCompulsory { get; set; }

   
}