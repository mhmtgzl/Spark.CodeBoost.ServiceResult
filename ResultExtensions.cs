using Microsoft.AspNetCore.Http;
using System.Net;

namespace Spark.CodeBoost.ServiceResult;


public static class ResultExtensions
{
    public static IResult ToResult(this Result result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(new { result.Message, result.Success }),
            HttpStatusCode.NotFound => Results.NotFound(new { result.Message, result.Success, result.Code }),
            HttpStatusCode.Unauthorized => Results.Json(new { result.Message, result.Success, result.Code }, statusCode: StatusCodes.Status401Unauthorized),
            HttpStatusCode.Forbidden => Results.Json(new { result.Message, result.Success, result.Code }, statusCode: StatusCodes.Status403Forbidden),
            HttpStatusCode.InternalServerError => Results.Json(new { result.Message, result.Success, result.Code }, statusCode: StatusCodes.Status500InternalServerError),
            _ => Results.BadRequest(new { result.Message, result.Success, result.Code })
        };
    }

    public static IResult ToResult<T>(this Result<T> result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(new { result.Message, result.Data, result.Success }),
            HttpStatusCode.NotFound => Results.NotFound(new { result.Message, result.Data, result.Success, result.Code }),
            HttpStatusCode.Unauthorized => Results.Json(new { result.Message, result.Data, result.Success, result.Code }, statusCode: StatusCodes.Status401Unauthorized),
            HttpStatusCode.Forbidden => Results.Json(new { result.Message, result.Data, result.Success, result.Code }, statusCode: StatusCodes.Status403Forbidden),
            HttpStatusCode.InternalServerError => Results.Json(new { result.Message, result.Data, result.Success, result.Code }, statusCode: StatusCodes.Status500InternalServerError),
            _ => Results.BadRequest(new { result.Message, result.Data, result.Success, result.Code })
        };
    }
}

