using System.ComponentModel.DataAnnotations;

namespace MessageBroker.Models;

public abstract class Topic
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public string? Name { get; init; } 
}