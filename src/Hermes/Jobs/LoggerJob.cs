using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Tristan.Core.Models;
using Tristan.Data.Gateways;

namespace Tristan.Jobs
{
    [DisallowConcurrentExecution]
    public class LoggerJob : IJob
    {
        private readonly ILogger<LoggerJob> _logger;


        private readonly IServiceProvider _provider;

        public LoggerJob(ILogger<LoggerJob> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Create a new scope
            using var scope = _provider.CreateScope();
            var contextGateway = scope.ServiceProvider.GetService<IContextGateway>();
            // Resolve the Scoped service
            if (contextGateway != null)
            {
                var tblMail = new List<Mail>
                {
                    new Mail
                    {
                        Id = Guid.Empty,
                        Oggetto = "fileName",
                    }
                };

                _logger.LogInformation("Writing Data", DateTimeOffset.Now);
                await contextGateway.SaveAsync(tblMail);

                _logger.LogInformation("Reading Data", DateTimeOffset.Now);
                var newTblMail = contextGateway.ReadAsync();
                _logger.LogInformation($"Reading Data: the filename is {newTblMail.FirstOrDefault()?.Oggetto}", DateTimeOffset.Now);

                _logger.LogInformation("Updating Data", DateTimeOffset.Now);
                tblMail.FirstOrDefault().Oggetto = "NewFilename";
                await contextGateway.UpdateAsync(tblMail);

                _logger.LogInformation("Reading Data", DateTimeOffset.Now);
                newTblMail = contextGateway.ReadAsync();
                _logger.LogInformation($"Reading Data: the filename is {newTblMail.FirstOrDefault()?.Oggetto}", DateTimeOffset.Now);

                _logger.LogInformation("Deleting Data", DateTimeOffset.Now);
                await contextGateway.DeleteAsync(tblMail);
            }
        }
    }
}