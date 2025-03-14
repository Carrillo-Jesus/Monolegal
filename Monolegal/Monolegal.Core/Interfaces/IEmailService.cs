using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monolegal.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailToClient(string recipient, string subject, string client, string code, string status);
    }
}
