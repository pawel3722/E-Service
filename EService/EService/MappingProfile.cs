using AutoMapper;
using EService.Dtos.ApplicationUserDtos;
using EService.Dtos.MessageDtos;
using EService.Dtos.ModelDtos;
using EService.Dtos.OrderDtos;
using EService.Dtos.PartDtos;
using EService.Dtos.ReviewDtos;
using EService.Dtos.RolesDtos;
using EService.Dtos.ServiceDtos;
using EService.Dtos.ServiceTypeDtos;
using EService.Models;

namespace EService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /*CreateMap<ApplicationUser, ReturnApplicationUserDto>().
                ForMember(u => CreateMap<Role, FieldsOnlyRoleDto>().
                                    ForMember(u => u.Id, r => r.MapFrom(s => s.Id)).
                                    ForMember(u => u.Name, r => r.MapFrom(s => s.Name)),
                                    r => r.MapFrom(s => s.Roles)).

                ForMember(u => u.SentMessages, r => r.MapFrom(s => s.SentMessages)).
                ForMember(u => u.ReceivedMessages, r => r.MapFrom(s => s.ReceivedMessages)).
                ForMember(u => u.CustomerOrders, r => r.MapFrom(s => s.CustomerOrders)).
                ForMember(u => u.ManagerOrders, r => r.MapFrom(s => s.ManagerOrders)).
                ForMember(u => u.Services, r => r.MapFrom(s => s.Services));*/

            CreateMap<ApplicationUser, ReturnApplicationUserDto>().
                             ForMember(u => CreateMap<Role, FieldsOnlyRoleDto>(), r => r.MapFrom(s => s.Roles)).
                             ForMember(u => CreateMap<Message, FieldsOnlyMessageDto>(), r => r.MapFrom(s => s.SentMessages)).
                             ForMember(u => CreateMap<Message, FieldsOnlyMessageDto>(), r => r.MapFrom(s => s.ReceivedMessages)).
                             ForMember(u => CreateMap<Order, FieldsOnlyOrderDto>(), r => r.MapFrom(s => s.CustomerOrders)).
                             ForMember(u => CreateMap<Order, FieldsOnlyOrderDto>(), r => r.MapFrom(s => s.ManagerOrders)).
                             ForMember(u => CreateMap<Service, FieldsOnlyServiceDto>(), r => r.MapFrom(s => s.Services));

            CreateMap<Message, ReturnMessageDto>().
                ForMember(u => u.SendingUser, r => r.MapFrom(s => s.SendingUser)).
                ForMember(u => u.ReceivingUser, r => r.MapFrom(s => s.ReceivingUser));

            CreateMap<Model, ReturnModelDto>().
                ForMember(u => u.Parts, r => r.MapFrom(s => s.Parts));

            CreateMap<Order, ReturnOrderDto>().
                ForMember(u => u.Customer, r => r.MapFrom(s => s.Customer)).
                ForMember(u => u.Manager, r => r.MapFrom(s => s.Manager)).
                ForMember(u => u.Review, r => r.MapFrom(s => s.Review)).
                ForMember(u => u.Services, r => r.MapFrom(s => s.Services));

            CreateMap<Part, ReturnPartDto>().
                ForMember(u => u.Service, r => r.MapFrom(s => s.Service)).
                ForMember(u => u.Model, r => r.MapFrom(s => s.Model));

            CreateMap<Review, ReturnReviewDto>().
                ForMember(u => u.Order, r => r.MapFrom(s => s.Order));

            CreateMap<Role, ReturnRoleDto>().
                ForMember(u => u.Users, r => r.MapFrom(s => s.Users));

            CreateMap<Service, ReturnServiceDto>().
                ForMember(u => u.Serviceman, r => r.MapFrom(s => s.Serviceman)).
                ForMember(u => u.Order, r => r.MapFrom(s => s.Order)).
                ForMember(u => u.ServiceType, r => r.MapFrom(s => s.ServiceType)).
                ForMember(u => u.Part, r => r.MapFrom(s => s.Part));

            CreateMap<ServiceType, ReturnServiceTypeDto>().
                ForMember(u => u.Services, r => r.MapFrom(s => s.Services));
        }
    }
}
