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
        this.logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            HttpContext httpContext = context.HttpContext;

            try
            {
                this.body = JsonConvert.SerializeObject(context.ActionArguments);
            }
            catch
            {
                // Do not have to handle this exception.
            }
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Function}", "OnActionExecuting");
        }

        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            this.HandleException(context);
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
            this.HandleCustomException(context, smsApiException);
        }
        else if (exception is Exception)
        {
            this.HandleException(context, exception);
        }

        context.ExceptionHandled = true;

        this.logger.LogError(context.Exception, "{Function}: Failed to process API calls. |  Body: {Body}", "HandleException", this.body);
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