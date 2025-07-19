using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using Services.Interfaces;

namespace Services.Implementations;

public class StoryService : IStoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IChatService _chatService;

    public StoryService(ApplicationDbContext dbContext, IChatService chatService)
    {
        _dbContext = dbContext;
        _chatService = chatService;
    }

    public async Task<Story> CreateStoryAsync(string theme, CancellationToken cancellationToken)
    {
        //TODO: Temporary, for tests
        var story =
            await _chatService.GenerateStoryAsync(theme, cancellationToken);

        return story;
        
        _dbContext.Stories.Add(story);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return story;
    }

    public Task<Story> GetStoryAsync(int storyId, CancellationToken cancellationToken)
    {
        // TODO: Make it work in Docker
        return _dbContext.Stories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == storyId, cancellationToken);
    }
}