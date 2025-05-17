using AutoMapper;
using TicketTracer.Api.Models.Common;
using TicketTracer.Data.Entities;

namespace TicketTracer.Api.Configuration;

internal static class MapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
    }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProjectEntity, ProjectModel>();
            CreateMap<BoardEntity, BoardModel>();
            CreateMap<TicketEntity, TicketModel>();
            CreateMap<UserEntity, UserModel>();
            CreateMap<AttributeEntity, AttributeModel>();
            CreateMap<AttributeValueEntity, AttributeValueModel>();
        }
    }
}