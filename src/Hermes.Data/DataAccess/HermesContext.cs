using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Data.DataAccess
{
    public class HermesContext : DbContext
    {
        public HermesContext(DbContextOptions options) : base(options) { }

        public DbSet<TblMails> Mail { get; set; }
        public DbSet<TblLastEmail> LastMail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mail Builder
            var mailBuilder = modelBuilder.Entity<TblMails>();

            mailBuilder
                .HasKey(mail => mail.Id);
            
            mailBuilder
                .Property(mail => mail.Subject)
                .HasMaxLength(maxLength: 250)
                .IsRequired();
            
            mailBuilder
                .Property(mail => mail.ReceivedDate)
                .IsRequired();
            
            mailBuilder
                .Property(mail => mail.Attachments)
                .HasMaxLength(maxLength: 2500)
                .IsRequired();
            
            #endregion
            #region LastMail Builder
            
            var lastMailBuilder = modelBuilder.Entity<TblLastEmail>();
            
            lastMailBuilder
                .HasKey(lm => lm.Id);

            lastMailBuilder
                .Property(lm => lm.ReceivedDate)
                .IsRequired();
            #endregion

        }
    }
}