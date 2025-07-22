using Contracts;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class StoryController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IStoryService _storyService;
    private static int Id = 1;

    public StoryController(IStoryService storyService, IChatService chatService)
    {
        _storyService = storyService;
        _chatService = chatService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var story = await _storyService.GetStoryAsync(int.Parse(id), cancellationToken);
        return Ok(story);
    }

    [HttpGet("{id}/complete")]
    public Task<IActionResult> GetComplete(string id, CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(Ok("Success"));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateStoryDto dto, CancellationToken cancellationToken)
    {
        var createdStory = await _storyService.CreateStoryAsync(dto.Theme, cancellationToken);

        return Ok(createdStory);
    }

    [HttpPost("{theme}")]
    public async Task<IActionResult> Test(string theme, CancellationToken cancellationToken)
    {
        var response = await _chatService.GenerateStoryAsync(theme, cancellationToken);

        return Ok(response);
    }
    

}