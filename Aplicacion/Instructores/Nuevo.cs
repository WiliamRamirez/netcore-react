using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepositorio;

            public Handler(IInstructor instructorRepositorio)
            {
                this._instructorRepositorio = instructorRepositorio;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructorModel = new InstructorModel
                {
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Titulo = request.Titulo                    
                };

                var resultado = await _instructorRepositorio.Post(instructorModel);
                
                if (resultado < 0)
                {
                    throw new Exception("No se pudo insertar el instructor");
                }
                
                return Unit.Value;

            }

        }
    }




}