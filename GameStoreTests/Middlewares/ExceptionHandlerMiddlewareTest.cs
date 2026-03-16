using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using GameStoreApi.Middlewares;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using GameStoreApi.Validation;
namespace GameStoreTests.Middlewares
{
    public class ExceptionHandlerMiddlewareTest
    {
        private readonly Mock<IExceptionMapper> mock;
        private readonly IEnumerable<IExceptionMapper> list;
        public ExceptionHandlerMiddlewareTest()
        {
            mock = new Mock<IExceptionMapper>();
            list = new List<IExceptionMapper> { mock.Object };
        }
        [Fact]
        public void EceptionHandler_ShouldGoNext()
        {
            var context = new DefaultHttpContext();
            var nextCalled = false;
            RequestDelegate next = (HttpContext ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };
            var middleware = new ExceptionHandlingMiddleware(next, list);
            middleware.InvokeAsync(context)?.Wait();
            Assert.True(nextCalled);
        }
        [Fact]
        public void ExceptionHandler_ShouldHandleException()
        {
            var context = new DefaultHttpContext();
            var exception = new Exception("Test exception");
            RequestDelegate next = (HttpContext ctx) =>
            {
                throw exception;
            };
            mock.Setup(m => m.CanMap(exception)).Returns(true);
            mock.Setup(m => m.Map(exception)).Returns(new ExceptionResponse(400, "Mapped exception"));
            var middleware = new ExceptionHandlingMiddleware(next, list);
            middleware.InvokeAsync(context)?.Wait();
            Assert.Equal(400, context.Response.StatusCode);
        }
        [Fact]
        public void ExceptionHandler_ShouldHandleExceptionWithoutMapper()
        {
            var context = new DefaultHttpContext();
            var exception = new Exception("Test exception");
            RequestDelegate next = (HttpContext ctx) =>
            {
                throw exception;
            };
            mock.Setup(m => m.CanMap(exception)).Returns(false);
            var middleware = new ExceptionHandlingMiddleware(next, list);
            middleware.InvokeAsync(context)?.Wait();
            Assert.Equal(500, context.Response.StatusCode);
        }
    }
}