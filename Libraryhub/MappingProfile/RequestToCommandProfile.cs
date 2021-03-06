﻿using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.MappingProfile
{
    public class RequestToCommandProfile : Profile
    {
        public RequestToCommandProfile()
        {
            CreateMap<AddBookRequestObj, AddBookCommand>();
            CreateMap<AddCheckOutActivityRequestObj, CheckOutCommand>();
            CreateMap<EditCheckOutActivityRequestObj, CheckInCommand>();
            CreateMap<OrderObj, OrderCommand>();
            CreateMap<OrderDetailsSearch, OrderDetailsSearchCommand>();
            CreateMap<ConfirmOrder, ConfirmOrderCommand>();
            CreateMap<OrderDetailsSearch, OrderDetailsSearchCommand>();
        }
    }
}
