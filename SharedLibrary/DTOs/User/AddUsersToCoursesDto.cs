using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs.User;

public class AddUsersToCoursesDto
{
    [Required(ErrorMessage = "Emails field required"), DataType(DataType.EmailAddress)]
    public string TeacherEmail { get; set; }

    [Required(ErrorMessage = "CourseFullName filed required")]

    public List<string> CoursesFullNames { get; set; }
}