namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        private readonly ILogger<CloudMailService> _logger;


        public CloudMailService(ILogger<CloudMailService> logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            _logger.LogInformation($"Mail From {_mailFrom} to {_mailTo}, " +
                $"with {nameof(CloudMailService)}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Message: {message}");


            Console.WriteLine($"Mail From {_mailFrom} to {_mailTo}, " +
                $"with {nameof(CloudMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
