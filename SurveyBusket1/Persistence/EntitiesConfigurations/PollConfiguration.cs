
namespace SurveyBusket8.Persistence.EntitiesConfigurations;

public class PollConfiguration:IEntityTypeConfiguration<poll>
{
    public void Configure(EntityTypeBuilder<poll> builder)
    {
        builder.HasIndex(x=>x.Title).IsUnique();
        builder.Property(p => p.Title).HasMaxLength(100);
        builder.Property(p => p.Summary).IsRequired().HasMaxLength(1500);
        
    }

    
}

