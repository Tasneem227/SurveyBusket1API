using Microsoft.AspNetCore.Http;
namespace SurveyBusket1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
               return Ok(_pollService.GetAll());
    }
    
    //api/polls/id
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var poll = _pollService.Get(id);
            
        //if (poll is null)
        //{
        //    return NotFound();
        //}
        //return Ok(poll);
        return poll is null? NotFound() : Ok(poll);
    }
    //add new poll
    [HttpPost("")]
    public IActionResult Add(poll request) { 
        var newPoll=_pollService.Add(request);
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll); 
    }

    //update poll
    [HttpPut("{id}")]
    public IActionResult Update(int id, poll request)
    {
        var isUpdated = _pollService.update(id, request);
        return isUpdated ? NoContent() : NotFound();

    }

    //delete poll
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = _pollService.Delete(id);
        return isDeleted ? NoContent() : NotFound();
    }

}
