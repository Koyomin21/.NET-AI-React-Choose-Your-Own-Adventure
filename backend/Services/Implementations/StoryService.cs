using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;
using Services.Interfaces;

namespace Services.Implementations;

public class StoryService : IStoryService
{
    private readonly ApplicationDbContext _dbContext;

    public StoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Story> CreateStoryAsync(Story story, CancellationToken cancellationToken)
    {
        _dbContext.Stories.Add(story);
        return _dbContext.SaveChangesAsync(cancellationToken)
            .ContinueWith(_ => story, cancellationToken);
    }

    public Task<Story> GetStoryAsync(int storyId, CancellationToken cancellationToken)
    {
        // TODO: Make it work in Docker
        return _dbContext.Stories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == storyId, cancellationToken);
    }
}