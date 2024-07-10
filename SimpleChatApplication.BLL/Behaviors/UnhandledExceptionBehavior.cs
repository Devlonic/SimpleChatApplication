using Ardalis.GuardClauses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace NexTube.Application.Common.Behaviours;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull {
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger) {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        try {
            return await next();
        }
        catch ( NotFoundException vex ) {
            var requestName = typeof(TRequest).Name;

            _logger.LogDebug(vex, "Request: Entity not found {Name} {@Request}", requestName, request);

            throw;
        }
        catch ( ValidationException vex ) {
            var requestName = typeof(TRequest).Name;

            _logger.LogDebug(vex, "Request: Validation fail at {Name} {@Request}", requestName, request);

            throw;
        }
        catch ( Exception ex ) {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning(ex, "Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
