using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Validation
{
    public interface IExceptionMapper
    {
        bool CanMap(Exception exception);
        ExceptionResponse Map(Exception exception);
    }
}
