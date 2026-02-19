using AutoMapper;
using OficiosYa.Application.DTOs.Response;
using OficiosYa.Domain.Entities;

namespace OficiosYa.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioResponse>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.ObtenerNombreCompleto()));
            
            CreateMap<Profesional, ProfesionalResponse>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.ObtenerNombreCompleto() : string.Empty));
        }
    }
}
