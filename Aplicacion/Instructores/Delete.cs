using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Delete
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }

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

                var resultado = await _instructorRepository.Delete(request.InstructorId);
                
                if (resultado < 0)
                {
                    throw new Exception("No pudo eliminar al instructor");
                }

                return Unit.Value;
            }
        }
    }
}