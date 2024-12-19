using MessageBroker.Data;
using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageBroker.Endpoints.Topics;

public static class Topics
{
    public static void RegisterTopicsEndpoints(this WebApplication app)
    {
        // TODO : Use a DTO instead model
        // TODO : Valid inputs
        app.MapGet("api/topics/{id:guid}", async (AppDbContext context, Guid id) =>
        {
            try
            {
                var topic = await context.Topics.FindAsync(id);
                if (topic == null)
                    Results.NotFound();
                return Results.Ok(topic);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        app.MapGet("api/topics/", async (AppDbContext context) =>
        {
            try
            {
                var topics = await context.Topics.ToListAsync();
                if (topics.Count == 0)
                    Results.NotFound();
                return Results.Ok(topics);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        app.MapPost("api/topics", async (AppDbContext context, Topic topic) =>
        {
            try
            {
                await context.Topics.AddAsync(topic);
                await context.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        app.MapPost("api/topics/{id:guid}/messages", async (AppDbContext context, Guid id, Message message) =>
        {
            try
            {
                var subscriptions = context.Subscriptions.Where(sub => sub.TopicId == id);
                if (!subscriptions.Any())
                    return Results.NotFound();
                var messageList = new List<Message>();
                foreach (var subscription in subscriptions)
                    messageList.Add(new Message
                    {
                        SubscriptionId = subscription.Id,
                        TopicMessage = message.TopicMessage,
                        ExpiresAfter = message.ExpiresAfter,
                        Status = message.Status
                    });
                context.Messages.AddRange(messageList);
                await context.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        app.MapGet("api/topics/messages", async (AppDbContext context) =>
        {
            try
            {
                var messages = await context.Messages.ToListAsync();
                if (messages.Count == 0)
                    Results.NotFound();
                return Results.Ok(messages);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
        app.MapPost("api/topics/{id:guid}/subscriptions", async (AppDbContext context, Guid id, Subscription subscription) =>
        {
            try
            {
                var isAnyTopic = context.Topics.Any(topic => topic.Id == id);
                if (!isAnyTopic)
                    return Results.NotFound();
                subscription.TopicId = id;
                context.Subscriptions.Add(subscription);
                await context.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }); 
        app.MapGet("api/topics/subscriptions", async (AppDbContext context) =>
        {
            try
            {
                var subscriptions = await context.Subscriptions.ToListAsync();
                if (subscriptions.Count == 0)
                    Results.NotFound();
                return Results.Ok(subscriptions);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        });
    }
    
}