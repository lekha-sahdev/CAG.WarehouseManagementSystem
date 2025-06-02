using CAG.WarehouseManagementSystem.ExceptionManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace CAG.WarehouseManagementSystem.Tests.ExceptionManagement
{
    public class CagExceptionFilterTests
    {
        private readonly Mock<ILogger<CagExceptionFilter>> _loggerMock;

        public CagExceptionFilterTests()
        {
            _loggerMock = new Mock<ILogger<CagExceptionFilter>>();
        }

        private ExceptionContext CreateExceptionContext(Exception ex)
        {
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
            };
            List<IFilterMetadata> filterData = new List<IFilterMetadata>();
			return new ExceptionContext(actionContext, filterData)
            {
                Exception = ex
            };
        }

        [Fact]
        public void OnException_CagBusinessException_SetsObjectResultWithStatusCode()
        {
            // Arrange
            var exception = new CagBusinessException("Business error", "Details", HttpStatusCode.BadRequest);
            var context = CreateExceptionContext(exception);
            var filter = new CagExceptionFilter(_loggerMock.Object);

            // Act
            filter.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsType<ErrorEntity>(result.Value);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_HttpRequestException_SetsObjectResultWithStatusCode()
        {
            // Arrange
            var exception = new HttpRequestException("Http error", null, HttpStatusCode.NotFound);
            var context = CreateExceptionContext(exception);
            var filter = new CagExceptionFilter(_loggerMock.Object);

            // Act
            filter.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.IsType<ErrorEntity>(result.Value);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_DbUpdateException_SetsObjectResultWithBadRequest()
        {
            // Arrange
            var dbUpdateEx = new DbUpdateException("DB error");
            var context = CreateExceptionContext(dbUpdateEx);
            var filter = new CagExceptionFilter(_loggerMock.Object);

            // Act
            filter.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsType<ErrorEntity>(result.Value);
            Assert.True(context.ExceptionHandled);
        }

        [Fact]
        public void OnException_UnknownException_SetsObjectResultWithInternalServerError()
        {
            // Arrange
            var exception = new Exception("Unknown error");
            var context = CreateExceptionContext(exception);
            var filter = new CagExceptionFilter(_loggerMock.Object);

            // Act
            filter.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.IsType<ErrorEntity>(result.Value);
            Assert.True(context.ExceptionHandled);
        }
    }
}
