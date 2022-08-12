using AutoMapper;
using HackChain.Core.Model;
using HackChain.Node.DTO;

namespace HackChain.Node.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<Transaction, TransactionWithBlockInfoDTO>()
                .ForMember(dest => dest.BlockIndex, opt => opt.MapFrom(source => source.Block.Index));

            CreateMap<Account, AccountDTO>();

            CreateMap<Block, BlockDTO>();
            CreateMap<BlockDTO, Block>();

            CreateMap<PeerNode, PeerNodeDTO>();
            CreateMap<PeerNodeDTO, PeerNode>();
        }
    }
}
