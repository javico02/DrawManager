using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Infrastructure
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _requestValidators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> requestValidators)
        {
            _requestValidators = requestValidators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Creating context validation for request
            var validationContext = new ValidationContext(request);

            // Getting validation failures in case that exist.
            var validationFailures = _requestValidators
                .Select(v => v.Validate(validationContext))
                .SelectMany(vr => vr.Errors)
                .Where(vf => vf != null);

            return validationFailures.Count() == 0
                ? await next()
                : throw new ValidationException(validationFailures);
        }
    }
}
