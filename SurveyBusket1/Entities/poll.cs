using SurveyBusket8.Contracts.Polls;

namespace SurveyBusket1.Entities;

public class poll:AuditableEntity 
{
    public int Id { get; set; }
    public string Title { get; set; }=string.Empty;
    public string Summary { get; set; }=string.Empty;
    public bool IsPublished { get; set; } 
    public DateTime StartsAt { get; set; } 
    public DateTime EndsAt { get; set; } 
    //public test test { get; set; } = default!;//don't know what ! means

    //public static explicit operator PollResponse(poll p)
    //{
    //    return new PollResponse
    //    {
    //        Id = p.Id,
    //        Title = p.Title,
    //        Description = p.Description
    //    };
    //}
}
//public class test
//{
//    public int Id { get; set; }
//    public string Title { get; set; } = string.Empty;
    

//}