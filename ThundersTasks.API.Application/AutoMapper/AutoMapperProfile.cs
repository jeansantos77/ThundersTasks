using AutoMapper;
using ThundersTasks.API.Domain.Dtos;
using ThundersTasks.API.Domain.Entities;

namespace ThundersTasks.API.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<TarefaDto, Tarefa>().ReverseMap();
        }
    }
}
