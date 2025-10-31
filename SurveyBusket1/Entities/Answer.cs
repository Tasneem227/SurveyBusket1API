namespace SurveyBusket1.Entities;

public sealed class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }=string.Empty;
    //Soft Delete
    public bool isActive { get; set; } = true;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = default!;
}
