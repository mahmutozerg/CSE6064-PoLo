﻿using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs.FileResult;
using SharedLibrary.DTOs.Responses;
using SharedLibrary.Models.business;

namespace PoLoAnalysisBusiness.Core.Services;

public interface IResultService:IGenericService<Result>
{
    public void SetFilePath(string filePath, string id);
    public Task AnalyzeExcel(Course course);

    public Task<CustomResponseDto<Result?>> AddAsync(string fileId, string path,string createdBy);

    public Task<byte[]> GetFileStreamAsync(string fileId);
}