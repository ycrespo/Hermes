using System.Linq;
using System.Threading.Tasks;
using Hermes.Core;
using Hermes.Core.Gateways;
using Hermes.Data.Gateways;
using Hermes.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Hermes.Jobs
{
    [DisallowConcurrentExecution]
    public class GetAttachmentsJob : IJob
    {
        private readonly ILogger<GetAttachmentsJob> _logger;
        private readonly IReadInboxGateway _maInboxGateway;
        private readonly IContextGateway _context;
        private readonly MailSettings _options;

        public GetAttachmentsJob(ILogger<GetAttachmentsJob> logger, IReadInboxGateway maInboxGateway, IContextGateway context, IOptions<MailSettings> options)
        {
            _logger = logger;
            _maInboxGateway = maInboxGateway;
            _context = context;
            _options = options.Value;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Getting the last email received");
            var lastEmail = await _context.GetInitialDateAsync();
            
            _logger.LogInformation("Download all the attachments from recent mails");
            var mails = _maInboxGateway.GetTblMails(
                  lastEmail.ReceivedDate,
                  _options.DestinationPath,
                  _options.Server, 
                  _options.UserName, 
                  _options.Password);

            if (!mails.Success.Any())
            {
                _logger.LogInformation("No new mails found");
                return;
            }
            
            _logger.LogInformation("Save downloaded mails");
            await _context.SaveAsync(mails.Success);

            _logger.LogInformation("Update last email received");
            lastEmail.ReceivedDate = mails.Success
                .OrderByDescending(lm => lm.ReceivedDate)
                .Select(lm => lm.ReceivedDate)
                .First();

            await _context.UpdateInitialDateAsync(lastEmail);
        }
    }
}