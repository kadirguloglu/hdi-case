using Microsoft.EntityFrameworkCore;
using BN = BCrypt.Net;

public class HdiDbContext : DbContext
{
    public DbSet<AdminLoginData> AdminLoginData { get; set; }
    public DbSet<Logging> Loggings { get; set; }

    public HdiDbContext()
    {

    }

    public HdiDbContext(DbContextOptions<HdiDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseChangeTrackingProxies(false, false);
            optionsBuilder.UseLazyLoadingProxies(false);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
            optionsBuilder.UseSqlServer(EnvironmentSettings.MssqlConnectionString, y =>
            {
                y.MigrationsAssembly("HdiCase.RestApi");
                y.CommandTimeout(1200);
                y.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            });
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

        modelBuilder.Entity<AdminLoginData>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasData(new AdminLoginData
            {
                Id = 1,
                Email = "superadmin@hdi.com",
                IsDeveloper = true,
                Password = BN.BCrypt.HashPassword("65a770b3-3363-4a67-9258-7d89207605f8-277c7e0b-8148-49ee-9141-5ec19d29cee5"),
                IsActive = true,
                RoleId = new int[] { 1, 2, 3 }
            });
        });

        modelBuilder.Entity<Logging>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}