using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.Abstractions;
using SurveyBusket1.Abstractions;
using SurveyBusket8.Contracts.Polls;

namespace SurveyBusket1.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]

public class PollsController(IPollService pollService,IMapper mapper) : ControllerBase
{
    private readonly IPollService _pollService = pollService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Route("GetAll")]
    
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls =await _pollService.GetAllAsync( cancellationToken);
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);  
    }

    //api/polls/id
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id
        , CancellationToken cancellationToken)
    {
        var result =await _pollService.GetAsync(id,  cancellationToken);
        return result.IsFailure ? Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description) : Ok(result.Value);
    }

    //add new poll
    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
        [FromServices] IValidator<PollRequest> validator,
        CancellationToken cancellationToken)
    {

        var newPoll = await _pollService.AddAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = newPoll.Value.Id }, newPoll.Value); //status code 201
    }

    //update poll
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id,
        [FromBody] PollRequest request
        , CancellationToken cancellationToken)
    {
        var result = await _pollService.updateAsync(id, request, cancellationToken);// implicit operator in CreatePollRequest class
        if (result.IsSuccess)
            return NoContent();

        return result.Error.Equals(PollErrors.DuplicatedPollTitle)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);

    }

    //delete poll
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id
        , CancellationToken cancellationToken)
    {
        var result = await _pollService.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
    }


    //Toggle publish status
    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);// implicit operator in CreatePollRequest class
        return isUpdated.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: isUpdated.Error.Code, detail: isUpdated.Error.Description);

    }

    //[HttpGet("Test")]
    //public IActionResult Test([FromQuery] int id, [FromQuery] poll request)
    //{
    //    //anonymous object 
    //    return Ok(request);
    //}
    //[HttpGet("Test")]
    //public IActionResult Test([FromHeader(Name ="x-lang")] string lang)
    //{

    //    return Ok(lang);
    //}
    //    [HttpGet("Test")]
    //    public IActionResult Test()
    //    {
    //        var stud = new Student()
    //        {
    //            Id = 1,
    //            FirstName = "mm",
    //            MiddleName = "jj",
    //            LastName = "jjj",
    //            dateofbirth = new DateTime(2003, 07, 29),
    //            Department = new Department()
    //            {
    //                Id = 1,
    //                Name = "ddddddd"
    //            }
    //        };
    //        var studresp = stud.Adapt<StudentResponse>();
    //        return Ok(studresp);
    //    }
    //}
    //[HttpPost("Test")]
    //public IActionResult Test([FromBody]Student std)
    //{

    //    return Ok("val ok");
    //}

}
