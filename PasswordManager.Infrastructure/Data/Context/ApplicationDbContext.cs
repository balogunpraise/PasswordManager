using Microsoft.EntityFrameworkCore;
using PasswordManager.Core.Domain.Entities;

namespace PasswordManager.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<LoginCredential> LoginCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
