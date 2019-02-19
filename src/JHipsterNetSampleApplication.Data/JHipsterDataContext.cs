
namespace JHipsterNetSampleApplication.Data.EntityFramework {
    using JHipsterNetSampleApplication.Data.EntityFramework.Identity;
    using JHipsterNetSampleApplication.Domain;
    using JHipsterNetSampleApplication.Domain.BankAccount;
    using JHipsterNetSampleApplication.Domain.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class JHipsterDataContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>,
        UserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>, IJHipsterDataContext {

        public JHipsterDataContext(DbContextOptions<JHipsterDataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.MapIdentity();
        }

        public DbSet<Operation> Operations { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<Domain.BankAccount.BankAccount> BankAccounts { get; set; }
    }
}
