namespace DutchTreat.Services
{
	public interface IMailService
	{
		void SendMesage(string to, string subject, string body);
	}
}