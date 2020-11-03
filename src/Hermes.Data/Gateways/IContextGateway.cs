using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core.Models;

namespace Hermes.Data.Gateways
{
    public interface IContextGateway
    {
        public  Task<TblLastEmail> GetInitialDateAsync();
        Task<IEnumerable<TblMails>> ReadMailAsync();

        Task<IEnumerable<TBase>> SaveAsync<TBase>(IEnumerable<TBase> allEntities)
            where TBase : EntityBase;
        Task<IEnumerable<TblMails>> UpdateMailsAsync(IEnumerable<TblMails> allMails);
        Task<IEnumerable<TblMails>> DeleteAsync(IEnumerable<TblMails> allEntities);

        Task<TblLastEmail> UpdateInitialDateAsync(TblLastEmail initialDate);
    }
}