using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using BN = BCrypt.Net;

// dotnet ef migrations add Initial --project HdiCase.RestApi
public class HdiDbContext : DbContext
{
    public DbSet<AdminLoginData> AdminLoginData { get; set; }
    public DbSet<Company> Company { get; set; }
    public DbSet<Logging> Logging { get; set; }
    public DbSet<Risk> Risk { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Aggrement> Aggrement { get; set; }
    public DbSet<AggrementAnalysis> AggrementAnalysis { get; set; }
    public DbSet<AggrementContact> AggrementContact { get; set; }
    public DbSet<AggrementFile> AggrementFile { get; set; }

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

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasData(new Company
            {
                Id = 1,
                AggrementResultWebhookUrl = "http://kadirguloglu.com/aggrementResult",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                ApiIsActive = true,
                ApiKey = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"),
                ApiPerMinuteMaximumRequestCount = 100,
                Emails = new string[] { "kadirguloglu1@gmail.com" },
                Name = "Test company",
                Phones = new string[] { "05074983810" }
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}