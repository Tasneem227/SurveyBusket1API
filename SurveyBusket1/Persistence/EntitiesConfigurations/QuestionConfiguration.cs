
namespace SurveyBusket1.Persistence.EntitiesConfigurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(a => a.Content).HasMaxLength(maxLength: 1000);
        builder.HasIndex(a => new { a.PollId, a.Content }).IsUnique(); // Can't have duplicate questions in the same poll
    }
}
