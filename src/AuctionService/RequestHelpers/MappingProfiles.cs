using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers;

public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.Item);
        CreateMap<Item,AuctionDto>();
        
        CreateMap<CreateAuctionDto,Auction>()
                .ForMember(d=>d.Item,o=>o.MapFrom(s=>s));
        CreateMap<CreateAuctionDto,Item>();

        //mapping the dto to the contract
        CreateMap<AuctionDto,AuctionCreated>();
        //when passing through the AuctionDto
        // CreateMap<AuctionDto,AuctionUpdated>();
        CreateMap<Auction,AuctionUpdated>().IncludeMembers(a=>a.Item);
        CreateMap<Item,AuctionUpdated>();
        
    }
}
