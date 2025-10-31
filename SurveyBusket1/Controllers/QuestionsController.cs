using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Abstractions;
using SurveyBusket1.Abstractions;

namespace SurveyBusket1.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _QuestionService = questionService;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId,int id, CancellationToken cancellationToken)
    {
        var result = await _QuestionService.GetAsync(pollId, id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _QuestionService.GellAllAsync(pollId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem(StatusCodes.Status404NotFound);
    }


    //Add Question
    [HttpPost]
    public async Task<IActionResult> AddQuestionAsync([FromRoute] int pollId, [FromBody] QuestionRequest QuestionRequest, CancellationToken cancellationToken)
    {
        var result = await _QuestionService.AddAsync(pollId, QuestionRequest, cancellationToken);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

        return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);
    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _QuestionService.UpdateAsync(pollId, id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
                ? result.ToProblem(StatusCodes.Status409Conflict)
                : result.ToProblem(StatusCodes.Status404NotFound);
    }

    [HttpPut("{Id}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, int id, CancellationToken cancellationToken)
    {
        var result = await _QuestionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem(StatusCodes.Status404NotFound);
    }
}
