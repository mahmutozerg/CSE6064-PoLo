﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Core.Models;

public class Result:Base
{
    [JsonIgnore]
    public File File { get; set; }
    public string FileId { get; set; }
    [Column(TypeName = "nvarchar(450)")]
    public string Path { get; set; }
    
    
}