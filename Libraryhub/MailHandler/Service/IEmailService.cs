using Libraryhub.Domain;
using Libraryhub.MailHandler;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Libraryhub.MailHandler.Service
{
    public interface IEmailService
    {
        Task Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
