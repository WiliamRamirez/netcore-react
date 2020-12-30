using System.Linq;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
                .ForMember(
                    // CursoDTO
                    dest => dest.Instructores,
                    // Curso
                    opt => opt.MapFrom(
                        src => src.InstructorLink.Select(a => a.Instructor).ToList()
                    )
                )
                .ForMember(dest => dest.Comentarios, opt => opt.MapFrom(src => src.ComentarioLista))
                .ForMember( dest => dest.Precio, opt => opt.MapFrom( src => src.PrecioPromocion ) );

            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();

            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<Precio, PrecioDTO>();

        }
    }
}