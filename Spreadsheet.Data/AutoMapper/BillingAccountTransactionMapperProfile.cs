using AutoMapper;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Entities;

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