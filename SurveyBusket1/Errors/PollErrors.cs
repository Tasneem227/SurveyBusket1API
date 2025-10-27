namespace SurveyBusket1.Errors;

public static class PollErrors
{
    public static Error PollNotFound =
        new Error("Poll.NotFound", "The specified poll was not found With the given id.");

    public static readonly Error DuplicatedPollTitle =
        new("Poll.DuplicatedTitle", "Another poll with the same title is already exists");
}
