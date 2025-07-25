using Persistence.Entities;

namespace Services.Interfaces;

public interface IStoryService
{
    Task<Story> GetStoryAsync(int storyId, CancellationToken cancellationToken);
    Task<Story> CreateStoryAsync(string theme, CancellationToken cancellationToken);
}