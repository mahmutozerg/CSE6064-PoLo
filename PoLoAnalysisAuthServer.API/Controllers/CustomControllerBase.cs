using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisAuthServer.Core.DTOs;
using PoLoAnalysisBusiness.DTO.Responses;

namespace PoLoAnalysisAuthServer.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomControllerBase:ControllerBase 
{
    [NonAction]
    public IActionResult CreateActionResult<T>(Core.DTOs.Response<T> res) where T : class
    {

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }
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