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

            CreateMap<Account, AccountDTO>();

            CreateMap<Block, BlockDTO>();

            CreateMap<PeerNode, PeerNodeDTO>();
            CreateMap<PeerNodeDTO, PeerNode>();
        }
    }
}
