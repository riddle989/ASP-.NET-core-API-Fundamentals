namespace CityInfo.API.services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}