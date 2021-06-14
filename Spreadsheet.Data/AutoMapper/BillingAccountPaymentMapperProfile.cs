using AutoMapper;
using Spreadsheet.Core.Dto.Data;
using Spreadsheet.Core.Entities;

namespace Spreadsheet.Data.AutoMapper
{
    public class BillingAccountPaymentMapperProfile : Profile
    {
        public BillingAccountPaymentMapperProfile()
        {
            CreateMap<BillingAccountPayment, BillingAccountPaymentDto>()
                .ReverseMap();
        }
    }
}