using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageBroker.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Topic> Topics => Set<Topic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(50);
            entity.Property(e => e.ExpiresAfter).HasColumnName("expires_after");
            entity.Property(e => e.TopicMessage).HasColumnName("topic_message").HasMaxLength(100);
            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
            entity.HasIndex(e => e.Id);
            entity.HasIndex(e => e.SubscriptionId);
        });
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(e => e.TopicId).HasColumnName("topic_id");
            entity.HasIndex(e => e.Id);
            entity.HasIndex(e => e.TopicId);
        });
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.HasIndex(e => e.Id);
        });
    }
}