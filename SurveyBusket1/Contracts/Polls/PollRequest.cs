//using SurveyBusket8.Contracts.Responses;

namespace SurveyBusket8.Contracts.Polls;

public record PollRequest
(
    string Title ,
     string Summary,
        DateTime StartsAt,
        DateTime EndsAt
);
    //public static explicit operator poll(CreatePollRequest p)
    //{
    //    return new poll
    //    {
    //        Title = p.Title,
    //        Description = p.Description
    //    };
    //}

