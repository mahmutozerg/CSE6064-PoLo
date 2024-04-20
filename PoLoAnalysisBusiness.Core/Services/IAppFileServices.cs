﻿using Microsoft.AspNetCore.Http;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary.DTO;
using WebProgrammingTerm.Auth.Core.DTOs;
using File = PoLoAnalysisBusiness.Core.Models.File;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IAppFileServices
{
     Task<CustomResponseDto<File>> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model);
     bool IsExcelFile(IFormFile file);

}