using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities;


//TODO: Search if the entity should have constraints
public class Story
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string SessionId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    [ForeignKey("RootNodeId")]
    public int? RootNodeId { get; set; }
    public StoryNode? RootNode { get; set; }

}