
namespace SurveyBusket1.Persistence.EntitiesConfigurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(a => a.Content).HasMaxLength(1000);
        builder.HasIndex(a => new { a.Content, a.QuestionId }).IsUnique(); // Can't have duplicate answers for the same question

    }
}
