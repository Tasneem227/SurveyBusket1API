using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace SurveyBusket8.Persistence;



public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options,
                                   IHttpContextAccessor httpContextAccessor ) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;

    public DbSet<poll> Polls { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entries = ChangeTracker.Entries<AuditableEntity>();
        foreach (var entityentry in entries) { 
            if(entityentry.State == EntityState.Added)
            {
                entityentry.Property(x => x.CreatedById).CurrentValue = currentUserId;
            }
            else if(entityentry.State == EntityState.Modified)
            {
                entityentry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                entityentry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
} 