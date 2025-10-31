namespace SurveyBusket1.Contracts.Questions;

public record QuestionRequest
(string Content,
    List<string> Answers
    );
