using System;
using System.Collections.Generic;
using Hermes.Core.Models;

namespace Hermes.Core.Gateways
{
    public interface IReadInboxGateway
    {
        IResult<IEnumerable<TblMails>, IEnumerable<Error>> GetTblMails(DateTime initialDate, string destinationPath, string server, string userName, string password);
    }
}