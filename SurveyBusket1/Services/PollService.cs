
namespace SurveyBusket1.Services;

public class PollService : IPollService
{
    private static readonly List<poll> _polls = [

       new poll { Id = 1, Title = "First Poll", Description = "This is the first poll." },
        new poll { Id = 2, Title = "Second Poll", Description = "This is the second poll." }
   ];

    public IEnumerable<poll> GetAll()=> _polls;
    public poll? Get(int id) => _polls.SingleOrDefault(x => x.Id == id);

    public poll Add(poll poll)
    {
        poll.Id = _polls.Count + 1;
        _polls.Add(poll);
        return poll;
    }

    public bool update(int id, poll poll)
    {
        var currentPoll = Get(id);
        if (currentPoll is null)
        {
            return false;
        }
        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;
        return true;
    }

    public bool Delete(int id)
    {
        var poll = Get(id);
        if (poll is null)
        {
            return false;
        }
         _polls.Remove(poll);
        return true;
    }
}
