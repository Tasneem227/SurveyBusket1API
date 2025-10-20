using System.Threading;

namespace SurveyBusket1.Services;

public interface IPollService
{
    Task<IEnumerable<poll>> GetAllAsync(CancellationToken cancellationToken);
    Task<poll?> GetAsync(int id, CancellationToken cancellationToken);
    Task<poll> AddAsync(poll poll,CancellationToken cancellationToken=default);
    Task<bool> updateAsync(int id, poll poll, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken);
}
