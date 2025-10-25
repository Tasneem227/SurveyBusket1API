namespace SurveyBusket1.Errors;

public static class PollErrors
{
    public static Error PollNotFound = new Error("Poll.NotFound", "The specified poll was not found With the given id.");
}
