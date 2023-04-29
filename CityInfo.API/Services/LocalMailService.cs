using CityInfo.API.Controllers;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        // we are using readonly, so that these fields value can't be modified after "LocalMailService" object is
        //created by the constructor.
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        private readonly ILogger<LocalMailService> _logger;


        public LocalMailService(ILogger<LocalMailService> logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
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
