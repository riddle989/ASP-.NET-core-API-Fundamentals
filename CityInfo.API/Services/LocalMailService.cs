using CityInfo.API.Controllers;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        public string _mailTo = "admin@mycompany.com";
        public string _mailFrom = "noreply@mycompany.com";

        private readonly ILogger<LocalMailService> _logger;


        public LocalMailService(ILogger<LocalMailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Send(string subject, string message)
        {
            _logger.LogInformation($"Mail From {_mailFrom} to {_mailTo}, " +
                $"with {nameof(LocalMailService)}");
            _logger.LogInformation($"Subject: {subject}");
            _logger.LogInformation($"Message: {message}");


            Console.WriteLine($"Mail From {_mailFrom} to {_mailTo}, " +
                $"with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
