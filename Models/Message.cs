using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBroker.Models;

public class Message
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public string? TopicMessage { get; init; }
    
    [Required]
    public DateTime ExpiresAfter { get; init; } = DateTime.Now;

    [Required] 
    public string Status { get; init; } = "New";
    
    [ForeignKey(nameof(Subscription.Id))]
    [Required]
    public Guid SubscriptionId { get; init; } 
}