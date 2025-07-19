using Persistence.Entities;

namespace Services.Interfaces;

public interface IChatService
{
    Task<Story> GenerateStoryAsync(string theme, CancellationToken cancellationToken);
}