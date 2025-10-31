
using SurveyBusket1.Contracts.Answers;

namespace SurveyBusket1.Contracts.Questions;

public record QuestionResponse
(int Id,string Content,IEnumerable<AnswerResponse> Answers
    );
