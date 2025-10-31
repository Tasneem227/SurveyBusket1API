using Microsoft.EntityFrameworkCore;
using SurveyBusket8.Persistence;

namespace SurveyBusket1.Services;

public class QuestionService(ApplicationDBContext context) : IQuestionService
{
    private readonly ApplicationDBContext _Context = context;

    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken)
    {
        var pollExists = await _Context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollExists) { 
        return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
        }
        var QuestionExists = await _Context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken);
        if (QuestionExists)
        {
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);
        }
        var question = request.Adapt<Question>();
        question.PollId = pollId;
        await _Context.Questions.AddAsync(question);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GellAllAsync(int pollId, CancellationToken cancellationToken)
    {
        var pollExists = await _Context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollExists)
        {
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
        }
        var questions = await _Context.Questions
            .Where(x => x.PollId == pollId)
            .AsNoTracking()
            //.Select(q => new QuestionResponse(
            //    q.Id,
            //    q.Content,
            //    q.Answers.Select(a => new AnswerResponse(a.Id, a.Content))
            //))
            .ProjectToType<QuestionResponse>()
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int Id, CancellationToken cancellationToken)
    {
        var question = await _Context.Questions
            .Where(x => x.PollId == pollId&& x.Id==Id)
            .AsNoTracking()
            .ProjectToType<QuestionResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        return question is null
            ? Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound)
            : Result.Success(question);
    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var questionIsExists = await context.Questions
            .AnyAsync(x => x.PollId == pollId
                && x.Id != id
                && x.Content == request.Content,
                cancellationToken
            );

        if (questionIsExists)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        var question = await context.Questions
            .Include(x => x.Answers)
            .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;

        //current answers
        var currentAnswers = question.Answers.Select(x => x.Content).ToList();

        //add new answer
        var newAnswers = request.Answers.Except(currentAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.isActive = request.Answers.Contains(answer.Content);
        });

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleStatusAsync(int pollId, int Id, CancellationToken cancellationToken)
    {
        var question = _Context.Questions
            .SingleOrDefault(x => x.PollId == pollId && x.Id == Id);

        if (question is null) return Result.Failure(QuestionErrors.QuestionNotFound);
        question.isActive = !question.isActive;

        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
