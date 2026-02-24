using GameStore.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; 
        var result = exception.Message;

        switch (exception)
        {
            case AlreadyExistsException:
                code = HttpStatusCode.Conflict;
                break;
            case NotFoundException:
                code = HttpStatusCode.NotFound; 
                break;
            case DbUpdateException dbEx when dbEx.InnerException is SqlException sqlEx:
                if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                {
                    code = HttpStatusCode.Conflict;
                    result = "Дані вже існують у системі (дублікат).";
                }
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsJsonAsync(new
        {
            error = result,
            status = (int)code
        });
    }
}