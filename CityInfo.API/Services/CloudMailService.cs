namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        public string _mailTo = "admin@mycompany.com";
        public string _mailFrom = "noreply@mycompany.com";

        private readonly ILogger<CloudMailService> _logger;


        public CloudMailService(ILogger<CloudMailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
