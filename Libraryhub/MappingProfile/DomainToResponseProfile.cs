using AutoMapper;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.DomainObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.MappingProfile
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Book, BookObj>()
                .ForMember(dest => dest.CheckOutActivities, opt => opt.MapFrom(src => src.CheckOutActivities));

            CreateMap<BooksActivity, CheckOutActivityObj>();

            CreateMap<BooksActivity, CheckOutActivityResponseObj>();

            CreateMap<BookPenalty, BookPenaltyObj>();

            CreateMap<Order, OrderObj>();

            CreateMap<OrderItem, OrderedBooks>();
        }
    }
}
