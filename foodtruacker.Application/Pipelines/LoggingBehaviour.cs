using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.Pipelines
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            _logger.LogInformation($"Handling {typeof(TRequest).Name} - requsted by {currentUser ?? "unknown"}");
            var response = await next();
            
            _logger.LogInformation($"Handled {typeof(TResponse).Name} - requsted by {currentUser ?? "unknown"}");
            return response;
        }
    }
}
