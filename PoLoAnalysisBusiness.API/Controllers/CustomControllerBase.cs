using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary.DTOs.Responses;

namespace PoLoAnalysisBusinessAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomControllerBase:ControllerBase 
{
    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponseDto<T> res)
    {

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }
    [NonAction]

    public IActionResult CreateActionResult<T>(CustomResponseListDataDto<T> res)
    {


        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }    
    [NonAction]

    public IActionResult CreateActionResult(CustomResponseNoDataDto res)
    {
        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }    

}