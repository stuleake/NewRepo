using System.Diagnostics.CodeAnalysis;
using TQ.Core.Models;

namespace UnitTest.Api.Admin.Core
{
    [ExcludeFromCodeCoverage]
    public static class UnitTestHelper<TEntity>
    {
        public static ServiceResponse<TEntity> GetServiceResponseSuccess(TEntity entity)
        {
            return new ServiceResponse<TEntity>
            {
                Code = (int)System.Net.HttpStatusCode.OK,
                Message = "success",
                OperationId = "operation id",
                Value = entity
            };
        }

        public static ServiceResponse<TEntity> GetServiceResponseBadRequest()
        {
            return new ServiceResponse<TEntity>
            {
                Code = (int)System.Net.HttpStatusCode.BadRequest,
                Message = "error",
                OperationId = "operation id",
            };
        }

        public static ServiceResponse<TEntity> GetServiceResponseInternalServerError()
        {
            return new ServiceResponse<TEntity>
            {
                Code = (int)System.Net.HttpStatusCode.InternalServerError,
                Message = "error",
                OperationId = "operation id",
            };
        }

        public static ServiceResponse<TEntity> GetServiceResponseCreated(TEntity entity)
        {
            return new ServiceResponse<TEntity>
            {
                Code = (int)System.Net.HttpStatusCode.Created,
                Message = "success",
                OperationId = "operation id",
                Value = entity
            };
        }
    }
}
