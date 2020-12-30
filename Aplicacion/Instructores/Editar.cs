using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _instructorRepository.Get(request.InstructorId);

                if (instructor == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "EL Instructor no existe" });
                }



                var InstructorModel = new InstructorModel
                {
                    InstructorId = request.InstructorId,
                    Nombre = request.Nombre ?? instructor.Nombre,
                    Apellidos = request.Apellidos ?? instructor.Apellidos,
                    Titulo = request.Titulo ?? instructor.Titulo
                };

                var resultado = await _instructorRepository.Put(InstructorModel);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se Guardaron los cambios en el curso");
  
            }
        }
    }
}