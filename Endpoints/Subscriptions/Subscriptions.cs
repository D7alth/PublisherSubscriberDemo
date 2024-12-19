using MessageBroker.Data;

namespace MessageBroker.Endpoints.Subscriptions;

public static class Subscriptions
{
    public static void RegisterSubscriptionsEndpoints(this WebApplication app)
    {
        // TODO : Use a DTO instead model
        // TODO : Valid inputs
        app.MapGet("api/subscriptions/{id:guid}/messages", async (AppDbContext context, Guid id) =>
        {
            try
            {
                var isAnySubscription = context.Subscriptions.Any(subscription => subscription.Id == id);
                if (!isAnySubscription)
                    return Results.NotFound();
                var messages =
                    context.Messages.Where(message => message.SubscriptionId == id && message.Status != "SENT");
                messages.ToList().ForEach(message => message.Status = "REQUESTED");
                await context.SaveChangesAsync();
                return Results.Ok(messages);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }); 
        app.MapPost("api/subscriptions/{id:guid}/messages",
            async (AppDbContext context, Guid id, Guid[] confirmedMessages) =>
        {
            try
            {
                var count = 0;
                var isAnySubscription = context.Subscriptions.Any(subscription => subscription.Id == id);
                if (!isAnySubscription)
                    return Results.NotFound();
                if (confirmedMessages.Length == 0)
                    return Results.BadRequest();
                confirmedMessages.ToList().ForEach(async messageId =>
                {
                    var message = context.Messages.FirstOrDefault(m => m.Id == messageId);
                    if (message is null) return;
                    message.Status = "SENT";
                    await context.SaveChangesAsync();
                    count++;
                });
                return Results.Ok($"Acknowledged {count}/{confirmedMessages.Length}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }); 
    }
}