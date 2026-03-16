using GameStoreApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameStoreApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IEnumerable<IExceptionMapper> mappers;

        public ExceptionHandlingMiddleware(RequestDelegate next,IEnumerable<IExceptionMapper> mappers)
        {
            this.next = next;
            this.mappers = mappers;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex,mappers);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IEnumerable<IExceptionMapper> mappers)
        {
            if (context.Response.HasStarted) return;

            var mapper = mappers.FirstOrDefault(m => m.CanMap(exception));

            var response = mapper?.Map(exception)
                ?? new ExceptionResponse(500, exception.InnerException?.Message ?? exception.Message);

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                error = response.Message,
                status = context.Response.StatusCode,
                traceId = context.TraceIdentifier
            });
        }
    }
}