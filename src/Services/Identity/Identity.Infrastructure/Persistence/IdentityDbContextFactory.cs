using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure;

public class IdentityDbContextFactory
    : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<IdentityDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=IdentityDb;User Id=sa;Password=Hcm@Passw0rd;TrustServerCertificate=True;Encrypt=False;");

        return new IdentityDbContext(optionsBuilder.Options);
    }
}