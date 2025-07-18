using Contracts;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entities;
using Services.Interfaces;

[ApiController]
[Route("[controller]")]
public class StoryController : ControllerBase
{

    private readonly IStoryService _storyService;
    private static int Id = 1;

    public StoryController(IStoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var story = await _storyService.GetStoryAsync(int.Parse(id), cancellationToken);
        return Ok(story);
    }

    [HttpGet("{id}/complete")]
    public async Task<IActionResult> GetComplete(string id, CancellationToken cancellationToken)
    {
        return Ok("Success");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateStoryDto dto, CancellationToken cancellationToken)
    {
        var story = new Story
        {
            Id = Id++,
            Title = dto.Theme,
            SessionId = Guid.NewGuid().ToString(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        var createdStory = await _storyService.CreateStoryAsync(story, cancellationToken);
        
        return Ok(createdStory);
    }
    

}