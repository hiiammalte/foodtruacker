using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web;

namespace foodtruacker.EmailService.Services
{
    public class DemoEmailService : IEmailService
    {
        private readonly ILogger<DemoEmailService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoEmailService(ILogger<DemoEmailService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task SendEmailAddressConfirmationLink(Guid userId, string userEmail, string userFullName, string token)
        {
            string encodedToken = HttpUtility.UrlEncode(token);
            string verificationUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/authentication/confirmEmail?userid={userId}&token={encodedToken}";

            //TODO: Implementing email service
            _logger.LogInformation($"Service would send email containing the following verification url: {verificationUrl}");

            return Task.CompletedTask;
        }
    }
}
