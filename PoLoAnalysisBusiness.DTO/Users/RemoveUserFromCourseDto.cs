﻿using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisBusiness.DTO.Users;

public class RemoveUserFromCourseDto
{
    [Required(ErrorMessage = "Emails field required"), DataType(DataType.EmailAddress)]
    public string TeacherEmail { get; set; }

    [Required(ErrorMessage = "CourseFullName filed required")]

    public string CourseFullName { get; set; }
}