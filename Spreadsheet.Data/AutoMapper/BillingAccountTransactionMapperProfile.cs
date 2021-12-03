using AutoMapper;
using Spreadsheet.Data.Dto;
using Spreadsheet.Data.Entities;

namespace Spreadsheet.Data.AutoMapper
{
    public class BillingAccountTransactionMapperProfile : Profile
    {
        public BillingAccountTransactionMapperProfile()
        {
            CreateMap<BillingAccountTransaction, BillingAccountTransactionDto>()
                .ReverseMap();
        }
    }
}