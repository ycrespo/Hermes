using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Data.DataAccess
{
    public class HermesContext : DbContext
    {
        public HermesContext(DbContextOptions options) : base(options) { }

        public DbSet<Mail> Mail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mail Builder
            var MailBuilder = modelBuilder.Entity<Mail>();

            MailBuilder
                .HasKey(Mail => Mail.Id);
            
            MailBuilder
                .Property(Mail => Mail.Oggetto)
                .HasMaxLength(maxLength: 100)
                .IsRequired();
            #endregion
        }
    }
}