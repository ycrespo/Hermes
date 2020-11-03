using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes.Core.Models;
using Hermes.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hermes.Data.Gateways
{
    public class ContextGateway : IContextGateway
    {
        private readonly HermesContext _context;
        private readonly ILogger<HermesContext> _logger;

        public ContextGateway(HermesContext context, ILogger<HermesContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TblLastEmail> GetInitialDateAsync() => await _context.LastMail.OrderByDescending(lm => lm.ReceivedDate).FirstOrDefaultAsync();
        public async Task<IEnumerable<TblMails>> ReadMailAsync() =>  await _context.Mail.ToListAsync();
        
        public async Task<IEnumerable<TBase>> SaveAsync<TBase>(IEnumerable<TBase> allEntities)  where TBase : EntityBase
        {
            var entities = allEntities.ToList();
            foreach (var entity in entities)
            {
                var inserted = await _context.AddAsync(entity);

                entity.Id = inserted.Entity.Id;
            }
            
            var result = await SaveChangesAsync("Cannot persist data to the database!!!");
            
            return result.HasError()
                ? new List<TBase>()
                : entities;
        }
        
        public async Task<IEnumerable<TblMails>> UpdateMailsAsync(IEnumerable<TblMails> allMails)
        {
            var mails = allMails.ToArray();
            foreach (var mail in mails)
            {
                _context.Mail.Update(mail);
            }
            
            var result = await SaveChangesAsync("Can not Update Mailuments in database!!!"); 

            return result.HasError()
                ? new List<TblMails>()
                : mails;
        }
        
        public async Task<TblLastEmail> UpdateInitialDateAsync(TblLastEmail initialDate)
        {
            _context.LastMail.Update(initialDate);
            
            var result = await SaveChangesAsync("Can not Update initial date in database!!!"); 

            return result.HasError()
                ? null
                : initialDate;
        }
        
        public async Task<IEnumerable<TblMails>> DeleteAsync(IEnumerable<TblMails> allMails)
        {
            var entities = allMails.ToList();

            _context.Mail.RemoveRange(entities);

            var result = await SaveChangesAsync("Can not delete Mailuments in database!!!");

            return result.HasError()
                ? new List<TblMails>()
                : entities;
        }
        
        private Task<IResult<int,Error.Exceptional>> SaveChangesAsync(string errorMessage)
        => Result.Try(
            async () => await _context.SaveChangesAsync(),
            ex => _logger.LogError(ex, errorMessage));

    }
}