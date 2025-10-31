using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace SurveyBusket8.Persistence;



public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options,
                                   IHttpContextAccessor httpContextAccessor ) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;

    public DbSet<Answer> Answers { get; set; } = default!;
    public DbSet<poll> Polls { get; set; } = default!;
    public DbSet<Question> Questions { get; set; } = default!;

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // ✅ Apply all entity configurations automatically
        // This loads every class in the assembly that implements IEntityTypeConfiguration<T>
        // so you don't need to manually call modelBuilder.ApplyConfiguration(...) for each one.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // ✅ Get all foreign key relationships in the model
        // Then filter only those that:
        //   - are NOT ownership relationships
        //   - and currently have DeleteBehavior set to Cascade
        var cascadesdFks = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        // ✅ Loop through all those foreign keys and change their delete behavior
        // from "Cascade" to "Restrict"
        foreach (var fk in cascadesdFks)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
        // Meaning:
        //   When a parent entity is deleted,
        //   EF Core will NOT automatically delete related child entities.
        //   Instead, it will prevent the delete if related data still exists.
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