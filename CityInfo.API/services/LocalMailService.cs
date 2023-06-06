namespace CityInfo.API.services
{
    public class LocalMailService : IMailService
    {
        private readonly ILogger _logger;
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public LocalMailService(
            ILogger<LocalMailService> logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            _logger.LogInformation($"Mail from {_mailFrom} to {_mailTo}" + $"with {nameof(LocalMailService)}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Message: {message}");
        }
    }
}
