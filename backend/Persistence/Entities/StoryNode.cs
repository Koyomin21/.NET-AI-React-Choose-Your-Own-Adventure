namespace Persistence.Entities;

public class StoryNode
{
    public int Id { get; private set; }
    public string Content { get; private set; }
    public bool IsEnding { get; private set; }
    public bool IsWinningEnding { get; private  set; }

    public IReadOnlyCollection<StoryOption> Options { get; private set; }

}