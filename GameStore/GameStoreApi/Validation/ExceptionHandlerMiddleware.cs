using GameStoreApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameStoreApi.Validation
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => this.next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                throw exception;
            }

            var status = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            switch (exception)
            {
                case OperationCanceledException:
                    status = (HttpStatusCode)499;
                    message = "Request was canceled.";
                    break;
                case AlreadyExistsException:
                    status = HttpStatusCode.Conflict;
                    message = exception.Message;
                    break;
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx:
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                    {
                        status = HttpStatusCode.Conflict;
                        message = "Data already exist.";
                    }
                    break;
                case ArgumentException argEx:
                    status = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
                case ValidationException valEx:
                    status = HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "Validation failed",
                        status = (int)status,
                        details = valEx.Errors, 
                        traceId = context.TraceIdentifier
                    });
                    return;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            await context.Response.WriteAsJsonAsync(new
            {
                error = message,
                status = context.Response.StatusCode,
                traceId = context.TraceIdentifier
            });
        }
    }
}