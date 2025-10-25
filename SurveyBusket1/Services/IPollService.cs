using System.Threading;

namespace SurveyBusket1.Services;

public interface IPollService
{
    Task<IEnumerable<poll>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<PollResponse>?> GetAsync(int id, CancellationToken cancellationToken);
    Task<Result<PollResponse>> AddAsync(PollRequest pollRequest,CancellationToken cancellationToken=default);
    Task<Result> updateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken);
}
