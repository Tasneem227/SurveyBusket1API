namespace SurveyBusket1.Entities;

public class AuditableEntity
{
    public String CreatedById { get; set; }= string.Empty;
    public DateTime CreatedOn { get; set; }= DateTime.UtcNow;
    public String? UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ApplicationUser CreatedBy { get; set; }=default!;
    public ApplicationUser? UpdatedBy { get; set; }
}
