using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core.Models;

namespace Hermes.Data.Gateways
{
    public interface IContextGateway
    {
        IEnumerable<Mail> ReadAsync();
        Task<IEnumerable<Mail>> SaveAsync(IEnumerable<Mail> Mails);
        Task<IEnumerable<Mail>> UpdateAsync(IEnumerable<Mail> Mails);
        Task<IEnumerable<Mail>> DeleteAsync(IEnumerable<Mail> Mails);
    }
}