using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Survey.API.Global_Exception_Handler;
using Survey.Domain.CustomException;

public class ExceptionResponseHandlingFilter : ActionFilterAttribute
{
    private readonly ILogger<ExceptionResponseHandlingFilter> logger;
    private string body;

    public ExceptionResponseHandlingFilter(ILogger<ExceptionResponseHandlingFilter> logger)
    {
        logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            HttpContext httpContext = context.HttpContext;

            try
            {
                body = JsonConvert.SerializeObject(context.ActionArguments);
            }
            catch
            {
                // Do not have to handle this exception.
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{Function}", "OnActionExecuting");
        }

        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            HandleException(context);
        }

        base.OnActionExecuted(context);
    }

    private void HandleException(ActionExecutedContext context)
    {
        Exception exception = context.Exception;
        while (exception?.InnerException != null)
        {
            exception = exception.InnerException;
        }

        if (exception is CustomException smsApiException)
        {
            HandleCustomException(context, smsApiException);
        }
        else if (exception is Exception)
        {
            HandleException(context, exception);
        }

        context.ExceptionHandled = true;

        logger.LogError(context.Exception, "{Function}: Failed to process API calls. |  Body: {Body}", "HandleException", body);
    }

    private void HandleException(ActionExecutedContext context, Exception exception)
    {
        context.Result = new ObjectResult(new ErrorDetails(ErrorResponseCode.InternalServerError, exception.Message))
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }

    private void HandleCustomException(ActionExecutedContext context, CustomException customException)
    {
        context.Result = new ObjectResult(new ErrorDetails((ErrorResponseCode)customException.ResponseCodeStatus, customException.ErrorMessage))
        {
            StatusCode = StatusCodes.Status400BadRequest,
        };
    }
}