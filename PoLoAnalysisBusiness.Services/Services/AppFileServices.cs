using Microsoft.AspNetCore.Http;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary;
using File = PoLoAnalysisBusiness.Core.Models.File;

namespace PoLoAnalysisBusiness.Services.Services;

public class AppFileServices:GenericService<File>,IAppFileServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericService<File> _genericService;

    public AppFileServices(IGenericRepository<File?> repository, IUnitOfWork unitOfWork, IGenericService<File> genericService) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _genericService = genericService;
        
    }

    public async Task<CustomResponseDto<File>> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model)
    {
        try
        {
            if (model == null || model.Length == 0)
                return CustomResponseDto<File>.Fail( ResponseCodes.BadRequest,FileConstants.FILENULL);


            if (!IsExcelFile(model))
                return CustomResponseDto<File>.Fail(ResponseCodes.BadRequest, FileConstants.FILEMUSTBEEXCEL);
            

            var fileName = $"../UploadedFiles/{Guid.NewGuid().ToString()}.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.CopyToAsync(stream);

            var file = new File()
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = "test",
                Path = fileName

            };
            var result =await _genericService.AddAsync(file,"mahmut");
            await _unitOfWork.CommitAsync();
            return result;
        }        
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing Excel file: {ex.Message}");
            return CustomResponseDto<File>.Fail(ResponseCodes.SystemFail,$"Error writing Excel file: {ex.Message}");
        }
    }

    public bool IsExcelFile(IFormFile file)
    {
        return file.ContentType == FileConstants.EXCELFILEFORMATEXTENTION;
        //Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);    
    }


}