using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MyBank.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyBank.Application.Behaviors
{
    internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : BaseResponse
    {
        private readonly IEnumerable<IValidator<TResponse>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TResponse>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationFailure[] validationFailures = Validate(request);
            if (validationFailures.Length == 0)
            {
                return await next();
            }

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(BaseResponse<>))
            {
                Type result = typeof(TResponse).GetGenericArguments()[0];
                Type element = typeof(BaseResponse<>).MakeGenericType(result);

                TResponse response = (TResponse)Activator.CreateInstance(element);
                AddValidationFailure(validationFailures, response);
                return response;
            }
            else
            {
                TResponse response = (TResponse)Activator.CreateInstance(typeof(BaseResponse<>));
                AddValidationFailure(validationFailures, response);
                return response;
            }
        }

        public ValidationFailure[] Validate(TRequest request)
        {
            var context = new ValidationContext<TRequest>(request);

            ValidationResult[] results = _validators.Select(x => x.Validate(context))
                .ToArray();

            ValidationFailure[] failures = results.Where(res => !res.IsValid)
                .SelectMany(x => x.Errors)
                .ToArray();

            return failures;
        }

        public void AddValidationFailure(ValidationFailure[] failures, BaseResponse response)
        {
            foreach (var failure in failures)
            {
                response.AddError(failure.PropertyName + " " + failure.ErrorMessage);
            }

            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
