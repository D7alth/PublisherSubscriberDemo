using System.ComponentModel.DataAnnotations;

namespace MessageBroker.Models;

public class Subscription
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public string? Name { get; init; }
    
    [Required]
    public Guid TopicId { get; init; }
}