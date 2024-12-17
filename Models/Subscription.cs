using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBroker.Models;

public abstract class Subscription
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public string? Name { get; init; }
    
    [ForeignKey(nameof(Topic.Id))] 
    [Required]
    public Guid TopicId { get; init; }
}