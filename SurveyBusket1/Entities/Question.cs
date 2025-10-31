namespace SurveyBusket1.Entities;

public sealed class Question:AuditableEntity
{
    public int Id { get; set; }
    public string Content { get; set; }=string.Empty;
    //Soft Delete
    public bool isActive { get; set; } = true;
    public int PollId { get; set; }
    public poll Poll { get; set; } = default!;
    public List<Answer> Answers { get; set; } = [];
}
