using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameStoreApi.Validation
{
    public record ExceptionResponse(int StatusCode,object Message);
    public class OperationCanceledExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is OperationCanceledException;
        public ExceptionResponse Map(Exception exception)=> new ExceptionResponse(499, "Request was canceled.");
        
    }
    public class AlreadyExistsExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is AlreadyExistsException;
        public ExceptionResponse Map(Exception exception)=>new ExceptionResponse(HttpStatusCode.Conflict.GetHashCode(), exception.Message);
        
    }
    public class NotFoundExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is NotFoundException;
        public ExceptionResponse Map(Exception exception)=>new ExceptionResponse(HttpStatusCode.NotFound.GetHashCode(), exception.Message);
     
    }
    public class DbUpdateExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        public ExceptionResponse Map(Exception exception)=>new ExceptionResponse(HttpStatusCode.Conflict.GetHashCode(), "Data already exist.");
        
    }
    public class DbForeignKeyExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) =>
            exception is DbUpdateException dbEx &&
            dbEx.InnerException?.Message.Contains("FK_") == true;

        public ExceptionResponse Map(Exception exception) =>
            new ExceptionResponse(400, "Related entity (Genre or Platform) not found.");
    }
    public class ArgumentExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is ArgumentException;
        public  ExceptionResponse Map(Exception exception)=>new ExceptionResponse(HttpStatusCode.BadRequest.GetHashCode(), exception.Message);
        
    }
    public class BadHttpRequestExceptionMapper : IExceptionMapper
    {
        public bool CanMap(Exception exception) => exception is BadHttpRequestException;
        public ExceptionResponse Map(Exception exception) =>
            new ExceptionResponse(400, "Invalid JSON format or request: " + exception.Message);
    }
    public class ValidationExceptionMapper : IExceptionMapper
    {
         public bool CanMap(Exception exception) => exception is ValidationException;

        public ExceptionResponse Map(Exception exception)
        {
            var valEx = exception as ValidationException; 

            return new ExceptionResponse(
                400,
                new
                {
                    error = "Validation failed",
                    details = valEx?.Errors 
                }
            );
        }
    }
}
