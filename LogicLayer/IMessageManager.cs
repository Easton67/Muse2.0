using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IMessageManager
    {
        Task SendEmail(string to, string subject, string body);
    }
}