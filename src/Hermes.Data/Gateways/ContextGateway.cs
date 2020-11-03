using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes.Core.Models;
using Hermes.Data.DataAccess;
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
        
        public IEnumerable<Mail> ReadAsync() =>  _context.Mail.Local;
        
        public async Task<IEnumerable<Mail>> SaveAsync(IEnumerable<Mail> Mails)
        {
            
            foreach (var Mail in Mails)
            {
                var inserted = await _context.AddAsync(Mail);

                Mail.Id = inserted.Entity.Id;
            }
            
            var result = await SaveChangesAsync("Cannot persist data to the database!!!");
            
            return result.HasError()
                ? new List<Mail>()
                : Mails;
        }
        
        public async Task<IEnumerable<Mail>> UpdateAsync(IEnumerable<Mail> Mails)
        {
            foreach (var Mail in Mails)
            {
                _context.Mail.Update(Mail);
            }
            
            var result = await SaveChangesAsync("Can not Update Mailuments in database!!!"); 

            return result.HasError()
                ? new List<Mail>()
                : Mails;
        }
        
        public async Task<IEnumerable<Mail>> DeleteAsync(IEnumerable<Mail> Mails)
        {
            var entities = Mails.ToList();

            _context.Mail.RemoveRange(entities);

            var result = await SaveChangesAsync("Can not delete Mailuments in database!!!");

            return result.HasError()
                ? new List<Mail>()
                : entities;
        }
        
        private Task<IResult<int,Error.Exceptional>> SaveChangesAsync(string errorMessage)
        => Result.Try(
            async () => await _context.SaveChangesAsync(),
            ex => _logger.LogError(ex, errorMessage));

    }
}