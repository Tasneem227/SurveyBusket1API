
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SurveyBusket8.Persistence;
using System.Threading.Tasks;

namespace SurveyBusket1.Services;

public class PollService(ApplicationDBContext context) : IPollService
{
    private readonly ApplicationDBContext _Context = context;
  
    public async Task<IEnumerable<poll>> GetAllAsync(CancellationToken cancellationToken)=>
        await _Context.Polls.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Result<PollResponse?>> GetAsync(int id, CancellationToken cancellationToken) {
        var poll = await _Context.Polls.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return poll is null ? Result.Failure<PollResponse?>(PollErrors.PollNotFound) : Result.Success<PollResponse?>(poll.Adapt<PollResponse>());
    }
        

    public async Task<Result<PollResponse>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var isExistingTitle = await _Context.Polls.AnyAsync(x => x.Title == pollRequest.Title, cancellationToken: cancellationToken);
        if (isExistingTitle)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

        var poll = pollRequest.Adapt<poll>();
        await _Context.Polls.AddAsync(poll, cancellationToken);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success(poll.Adapt<PollResponse>());
    }

    public async Task<Result> updateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
    {
        var isExistingTitle = await _Context.Polls.AnyAsync(x => x.Title == poll.Title && x.Id!=id,cancellationToken);
        if(isExistingTitle)
            return Result.Failure(PollErrors.DuplicatedPollTitle);

        var currentPoll = await _Context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (currentPoll is null)
        {
            return Result.Failure(PollErrors.PollNotFound);
        }
        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;
        _Context.Update(currentPoll);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
        ;
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _Context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (poll is null)
        {
            return Result.Failure(PollErrors.PollNotFound);
        }
        _Context.Remove(poll);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await _Context.Polls.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (poll is null)
        {
            return Result.Failure(PollErrors.PollNotFound);
        }
        poll.IsPublished = !poll.IsPublished;
        _Context.Update(poll);
        await _Context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
