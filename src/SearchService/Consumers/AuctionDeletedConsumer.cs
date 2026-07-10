using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer:IConsumer<AuctionDeleted>{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        System.Console.WriteLine($"consuming delete-event-with-auction-Id {context.Message.Id}");
        var entityId=context.Message.Id;
        var result=await DB.DeleteAsync<Item>(entityId);
        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionDeleted),"Problem deleting auction");
        }
    }
}