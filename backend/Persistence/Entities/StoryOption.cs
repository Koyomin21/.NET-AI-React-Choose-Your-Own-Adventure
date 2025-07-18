using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Entities;

public class StoryOption
{
    public int Id { get; set; }
    public string Text { get; set; }

    [ForeignKey("NextNodeId")]
    public int NextNodeId { get; set; }
    public StoryNode NextNode { get; set; }
}