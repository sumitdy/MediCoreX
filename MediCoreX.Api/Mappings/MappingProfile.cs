using AutoMapper;
using MediCoreX.Api.Models;
using MediCoreX.Api.DTOs;

namespace MediCoreX.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
        }
    }
}
