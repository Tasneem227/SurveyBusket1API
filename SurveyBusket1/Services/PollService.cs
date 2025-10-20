
using SurveyBusket8.Persistence;
using System.Threading.Tasks;

namespace SurveyBusket1.Services;

public class PollService(ApplicationDBContext context) : IPollService
{
    private readonly ApplicationDBContext _Context = context;
    private static readonly List<poll> _polls = [

       new poll { Id = 1, Title = "First Poll", Summary = "This is the first poll." },
        new poll { Id = 2, Title = "Second Poll", Summary = "This is the second poll." }
   ];

    public async Task<IEnumerable<poll>> GetAllAsync(CancellationToken cancellationToken)=>
        await _Context.Polls.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<poll?> GetAsync(int id, CancellationToken cancellationToken) =>
        await _Context.Polls
            .FindAsync(id,cancellationToken);

    public async Task<poll> AddAsync(poll poll, CancellationToken cancellationToken = default)
    {

        await _Context.AddAsync(poll,cancellationToken);
        await _Context.SaveChangesAsync(cancellationToken);
        return poll;
    }

    public async Task<bool> updateAsync(int id, poll poll, CancellationToken cancellationToken)
    {
        var currentPoll =await GetAsync(id, cancellationToken);
        if (currentPoll is null)
        {
            return false;
        }
        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;
        _Context.Update(currentPoll);
        await _Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var poll =await GetAsync(id, cancellationToken);
        if (poll is null)
        {
            return false;
        }
        _Context.Remove(poll);
        await _Context.SaveChangesAsync(cancellationToken);
        return true;
    }
     public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await GetAsync(id, cancellationToken);
        if (poll is null)
        {
            return false;
        }
        poll.IsPublished = !poll.IsPublished;
        _Context.Update(poll);
        await _Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
