using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Comentario;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string ComentarioTexto { get; set; }
            public Guid CursoId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Alumno).NotEmpty();
                RuleFor(x => x.Puntaje).NotEmpty();
                RuleFor(x => x.ComentarioTexto).NotEmpty();
                RuleFor(x => x.CursoId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly IComentario _comentarioRepository;

            public Handler(IComentario comentarioRepository)
            {
                this._comentarioRepository = comentarioRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentarioModel = new ComentarioModel
                {
                    Alumno = request.Alumno,
                    Puntaje = request.Puntaje,
                    ComentarioTexto = request.ComentarioTexto,
                    CursoId = request.CursoId
                };

                var resultado = await _comentarioRepository.Post(comentarioModel);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo agregar el comentario");
            }
        }

    }
}