using System.Collections.Generic;
using System.Threading.Tasks;
using Tristan.Core.Models;

namespace Tristan.Data.Gateways
{
    public interface IContextGateway
    {
        IEnumerable<Mail> ReadAsync();
        Task<IEnumerable<Mail>> SaveAsync(IEnumerable<Mail> Mails);
        Task<IEnumerable<Mail>> UpdateAsync(IEnumerable<Mail> Mails);
        Task<IEnumerable<Mail>> DeleteAsync(IEnumerable<Mail> Mails);
    }
}