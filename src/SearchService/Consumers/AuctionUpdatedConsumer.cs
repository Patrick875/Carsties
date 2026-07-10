using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;
    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper=mapper;
    }
    public async Task Consume (ConsumeContext<AuctionUpdated> context)
    {

        var message= context.Message;
        
        var mappedMessage= _mapper.Map<Item>(message);

        // should only be use only when Item is identical to AuctionUpdated

        // await DB.Update<Item>()
        //     .MatchID(message.Id)
        //     .ModifyWith(mappedMessage)
        //     .ExecuteAsync();

        // updates with only properties present on the mappedMessage

      var result=  await DB.Update<Item>()
            .MatchID(message.Id)
            .ModifyOnly(x => new
            {
                x.Make,
                x.Model,
                x.Year,
                x.Color,
                x.Mileage
            },mappedMessage)
            .ExecuteAsync();
        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionUpdated),"Problem updating mongodb");
        }
    }
}