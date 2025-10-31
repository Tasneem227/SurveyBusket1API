namespace SurveyBusket1.Services;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> AddAsync (int pollId , QuestionRequest request,CancellationToken cancellationToken);

    Task<Result<IEnumerable<QuestionResponse>>> GellAllAsync (int pollId , CancellationToken cancellationToken);
    Task<Result<QuestionResponse>> GetAsync (int pollId ,int Id ,CancellationToken cancellationToken);
    Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(int pollId, int Id, CancellationToken cancellationToken);
}
