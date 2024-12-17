using System.ComponentModel.DataAnnotations;

namespace MessageBroker.Models;

public class Topic
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public string? Name { get; init; } 
}