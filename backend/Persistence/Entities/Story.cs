using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities;


public class Story
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string SessionId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    [ForeignKey("RootNodeId")]
    public int RootNodeId { get; set; }
    public StoryNode RootNode { get; private set; }

}