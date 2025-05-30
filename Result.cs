using Spark.CodeBoost.ApplicationExceptions;
using System.Net;

namespace Spark.CodeBoost.ServiceResult;

public class Result
{
    public virtual bool Success { get; private set; }
    public virtual string Message { get; private set; }
    public virtual string Code { get; private set; }
    public virtual HttpStatusCode StatusCode { get; private set; }

    protected Result(bool success, string message, HttpStatusCode statusCode, string code = "")
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
        Code = code;
    }

    public static Result SuccessResult(string message = "OK")
    {
        return new Result(true, message, HttpStatusCode.OK);
    }

    public static Result FailureResult(string message = "")
    {
        return new Result(false, message, HttpStatusCode.BadRequest);
    }

    public static Result FailureResult(AppException exception)
    {
        return CreateFailureResultFromException(exception);
    }

    public static Result FailureResult(string message, params string[] parameters)
    {
        return new Result(false, FormatMessage(message, parameters), HttpStatusCode.BadRequest);
    }

    public static Result FailureResult(AppException exception, params string[] parameters)
    {
        return CreateFailureResultFromException(exception, parameters: parameters);
    }

    public static Result NotFoundResult(string message = "Resource not found.")
    {
        return new Result(false, message, HttpStatusCode.NotFound);
    }

    public static Result NotFoundResult(AppException exception)
    {
        return CreateFailureResultFromException(exception, HttpStatusCode.NotFound);
    }

    public static Result UnauthorizedResult(string message = "Unauthorized")
    {
        return new Result(false, message, HttpStatusCode.Unauthorized); // 401
    }

    public static Result Forbidden(string message = "Forbidden")
    {
        return new Result(false, message, HttpStatusCode.Forbidden); // 403
    }

    public static Result UnauthorizedResult(AppException exception)
    {
        return CreateFailureResultFromException(exception, HttpStatusCode.Unauthorized); // 401
    }

    public static Result Forbidden(AppException exception)
    {
        return CreateFailureResultFromException(exception, HttpStatusCode.Forbidden); // 403
    }

    public static Result InternalServerError(string message = "Internal Server Error")
    {
        return new Result(false, message, HttpStatusCode.InternalServerError);
    }

    public static Result SetFromResult(Result result)
    {
        return new Result(result.Success, result.Message, result.StatusCode, result.Code);
    }
    private static Result CreateFailureResultFromException(AppException exception, HttpStatusCode? overrideStatusCode = null, params string[] parameters)
    {
        string code = exception is CoreException coreException ? $"{coreException.Module.Code}-{coreException.ErrorCode}" : string.Empty;
        string message = parameters.Length > 0 ? FormatMessage(exception.ToString(), parameters) : exception.ToString();
        HttpStatusCode statusCode = overrideStatusCode ?? (HttpStatusCode)exception.HttpStatusCode;

        return new Result(false, message, statusCode, code);
    }

    private static string FormatMessage(string message, string[] parameters)
    {
        return parameters.Length == 0 ? message : string.Format(message, parameters);
    }
}

public class Result<T> : Result
{
    public T? Data { get; private set; }

    private Result(bool success, string message, T? data, HttpStatusCode statusCode, string code = "")
        : base(success, message, statusCode, code)
    {
        Data = data;
    }

    public static Result<T> SuccessResult(T data, string message = "OK")
    {
        return new Result<T>(true, message, data, HttpStatusCode.OK);
    }

    public static Result<T> FailureResult(string message = "")
    {
        return new Result<T>(false, message, default, HttpStatusCode.BadRequest);
    }

    public static Result<T> FailureResult(AppException exception)
    {
        return CreateFailureResultFromException<T>(exception);
    }

    public static Result<T> FailureResult(string message, params string[] parameters)
    {
        return new Result<T>(false, FormatMessage(message, parameters), default, HttpStatusCode.BadRequest);
    }

    public static Result<T> FailureResult(AppException exception, params string[] parameters)
    {
        return CreateFailureResultFromException<T>(exception, parameters: parameters);
    }

    public static Result<T> NotFoundResult(string message = "Resource not found.")
    {
        return new Result<T>(false, message, default, HttpStatusCode.NotFound);
    }

    public static Result<T> UnauthorizedResult(string message = "Unauthorized")
    {
        return new Result<T>(false, message, default, HttpStatusCode.Unauthorized); // 401
    }

    public static Result<T> Forbidden(string message = "Forbidden")
    {
        return new Result<T>(false, message, default, HttpStatusCode.Forbidden); // 403
    }

    public static Result<T> UnauthorizedResult(AppException exception)
    {
        return CreateFailureResultFromException<T>(exception, HttpStatusCode.Unauthorized); // 401
    }

    public static Result<T> Forbidden(AppException exception)
    {
        return CreateFailureResultFromException<T>(exception, HttpStatusCode.Forbidden); // 403
    }

    public static Result<T> NotFoundResult(AppException exception)
    {
        return CreateFailureResultFromException<T>(exception, HttpStatusCode.NotFound);
    }
    public static Result<T> InternalServerError(string message = "Internal Server Error.")
    {
        return new Result<T>(false, message, default, HttpStatusCode.InternalServerError);
    }
    public static Result<T> SetFromResult(Result<T> result)
    {
        return new Result<T>(result.Success, result.Message, result.Data, result.StatusCode, result.Code);
    }
    private static Result<T> CreateFailureResultFromException<T>(AppException exception, HttpStatusCode? overrideStatusCode = null, params string[] parameters)
    {
        string code = exception is CoreException coreException ? $"{coreException.Module.Code}-{coreException.ErrorCode}" : string.Empty;
        string message = parameters.Length > 0 ? FormatMessage(exception.ToString(), parameters) : exception.ToString();
        HttpStatusCode statusCode = overrideStatusCode ?? (HttpStatusCode)exception.HttpStatusCode;

        return new Result<T>(false, message, default, statusCode, code);
    }

    private static string FormatMessage(string message, string[] parameters)
    {
        return parameters.Length == 0 ? message : string.Format(message, parameters);
    }
}
