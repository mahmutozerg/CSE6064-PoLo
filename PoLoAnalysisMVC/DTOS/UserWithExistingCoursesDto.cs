using SharedLibrary.Models.business;

namespace PoLoAnalysisMVC.DTOS;

public class UserWithExistingCoursesDto
{
    public AppUser AppUser { get; set; }
    public List<Course> FreeCourses { get; set; }
    public List<Course> Courses { get; set; }
    
}

