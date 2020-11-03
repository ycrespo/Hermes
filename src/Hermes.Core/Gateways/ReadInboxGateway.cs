using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EAGetMail;
using Hermes.Core.Models;
using Microsoft.Extensions.Logging;

namespace Hermes.Core.Gateways
{
    public class ReadInboxGateway : IReadInboxGateway
    {
        private readonly ILogger<ReadInboxGateway> _logger;

        public ReadInboxGateway(ILogger<ReadInboxGateway> logger)
        {
            _logger = logger;
        }

        public IResult<IEnumerable<TblMails>, IEnumerable<Error>> GetTblMails(DateTime initialDate, string destinationPath, string server, string userName, string password)
        {
            var success = new List<TblMails>();
            var errors = new List<Error>();
            // If the folder is not existed, create it.
            if (!Directory.Exists(destinationPath))
            {
                var createDir = Result.Try(
                    () => Directory.CreateDirectory(destinationPath),
                    ex => _logger.LogError(ex, "Error while downloading the attachments"));
                if (createDir.HasError())
                {
                    errors.Add(createDir.Error);
                    return Result<IEnumerable<TblMails>>.Error(errors);
                }
            }

            // Gmail IMAP4 server is "imap.gmail.com"
            var mailServer = new MailServer(server,
                userName,
                password,
                ServerProtocol.Imap4) { SSLConnection = true, Port = 993 };

            var mailClient = new MailClient("TryIt");

            var connect = Result.Try(
                () => mailClient.Connect(mailServer),
                ex => _logger.LogError(ex, $"Error while connecting to the server {server}"));

            if (connect.HasError())
            {
                errors.Add(connect.Error);
                return Result<IEnumerable<TblMails>>.Error(errors);
            }

            // retrieve unread/new email only
            mailClient.GetMailInfosParam.Reset();
            mailClient.GetMailInfosParam.GetMailInfosOptions = GetMailInfosOptionType.NewOnly;

            var mailInfos = mailClient.GetMailInfos();

            foreach (var mailInfo in mailInfos)
            {
                // Receive email from IMAP4 server
                var downloadedMail = mailClient.GetMail(mailInfo);

                if (downloadedMail.ReceivedDate.Subtract(initialDate).TotalMilliseconds <= 0 || downloadedMail.Attachments.Length == 0)
                {
                    //if (!info.Read)
                    //{
                    //    oClient.MarkAsRead(info, true);
                    //}
                    continue;
                }

                //Save attachments
                errors.AddRange(from att in downloadedMail.Attachments
                    select Result.Try(
                        () => att.SaveAs(Path.Combine(destinationPath, GetNewFileName(downloadedMail.Subject, att.Name)), overwrite: true),
                        ex => _logger.LogError(ex, $"Error saving attachment {att.Name}"))
                    into saveAtt
                    where saveAtt.HasError()
                    select saveAtt.Error);

                var mail = new TblMails
                {
                    Subject = downloadedMail.Subject,
                    ReceivedDate = downloadedMail.ReceivedDate,
                    Attachments = string.Join(";", downloadedMail.Attachments.Select(att => att.Name))
                };

                success.Add(mail);
            }

            // Quit and expunge emails marked as deleted from IMAP4 server.
            mailClient.Quit();
            return new Result<IEnumerable<TblMails>, IEnumerable<Error>>(success, errors);
        }

        private string GetNewFileName(string subject, string filename) =>
            // Get practice number form subject
            // Get other things
            filename;
    }
}