using Contracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class StoryController : ControllerBase
{


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        return Ok("Success");
    }

    [HttpGet("{id}/complete")]
    public async Task<IActionResult> GetComplete(string id, CancellationToken cancellationToken)
    {
        return Ok("Success");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateStoryDto dto)
    {
        return Ok();
    }
    

}