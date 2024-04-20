using Microsoft.AspNetCore.Http;
using PoLoAnalysisBusiness.Core.Services;
using SharedLibrary;
using SharedLibrary.DTO;

namespace PoLoAnalysisBusiness.Services.Services;

public class AppFileServices:IAppFileServices
{
    public async Task<CustomResponseNoDataDto> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model)
    {
        try
        {
            if (model == null || model.Length == 0)
                return CustomResponseNoDataDto.Fail(ResponseCodes.BadRequest, FileConstants.FILENULL);


            if (!IsExcelFile(model))
                return CustomResponseNoDataDto.Fail(ResponseCodes.BadRequest, FileConstants.FILEMUSTBEEXCEL);
            

            var fileName = $"../UploadedFiles/{Guid.NewGuid().ToString()}.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.CopyToAsync(stream);

            return CustomResponseNoDataDto.Success(ResponseCodes.Ok);
        }        
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing Excel file: {ex.Message}");
            return CustomResponseNoDataDto.Fail(ResponseCodes.SystemFail,$"Error writing Excel file: {ex.Message}");
        }
    }

    public bool IsExcelFile(IFormFile file)
    {
        return file.ContentType == FileConstants.EXCELFILEFORMATEXTENTION;
        //Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);    
    }
}