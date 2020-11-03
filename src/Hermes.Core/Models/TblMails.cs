using System;

namespace Hermes.Core.Models
{
    public class TblMails : EntityBase
    {
        public string Subject { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Attachments { get; set; }
    }
}