namespace SurveyBusket1.Services;

public interface IPollService
{
    IEnumerable<poll> GetAll();
    poll Get(int id);
    poll Add(poll poll);
    bool update(int id, poll poll);
    bool Delete(int id);
}
