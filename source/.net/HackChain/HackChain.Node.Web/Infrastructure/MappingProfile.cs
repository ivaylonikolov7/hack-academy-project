﻿using AutoMapper;
using HackChain.Core.Model;
using HackChain.Node.Web.DTO;

namespace HackChain.Node.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();
        }
    }
}