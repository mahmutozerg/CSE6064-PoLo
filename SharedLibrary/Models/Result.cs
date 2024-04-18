﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models;

public class Result:Base
{
    public File File { get; set; }
    public string FileId { get; set; }
    [Column(TypeName = "nvarchar(450)")]
    public string Path { get; set; }
    
    
}