namespace Survey.API.Global_Exception_Handler
{
    using Microsoft.AspNetCore.Diagnostics;
    using Survey.Domain.CustomException;
    using System.Net;

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error != null && contextFeature.Error is CustomException)
                        {
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = ((CustomException)contextFeature.Error).ResponseCodeStatus,
                                Message = ((CustomException)contextFeature.Error).ErrorMessage,
                            }.ToString());
                        }
                        else
                        {
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "Exception has occured",
                            }.ToString());
                        }
                    }
                });
            });
        }

    }
}
