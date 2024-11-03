using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBank.Domain.Response;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Behaviors
{
    internal class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : BaseResponse
    {
		private readonly ILogger _logger;

        public ExceptionHandlingBehavior(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				_logger.Warning(ex.Message, ex);

				if (ex.InnerException != null)
				{
                    _logger.Warning(ex.InnerException.Message, ex);
                }
				if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(BaseResponse<>))
				{
					Type resultType = typeof(TResponse).GetGenericArguments()[0];
					Type elementType = typeof(BaseResponse<>).MakeGenericType(resultType);

					TResponse result = (TResponse)Activator.CreateInstance(elementType);

					result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					result.AddError(ex.Message + " " + ex.StackTrace);
                    return result;
                }
                else
				{
                    TResponse result = (TResponse)Activator.CreateInstance(typeof(BaseResponse<>));

                    result.StatusCode = System.Net.HttpStatusCode.InternalServerError;
					result.AddError(ex.Message);
					return result;
                }
			}
        }
    }
}
