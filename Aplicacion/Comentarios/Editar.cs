using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Comentario;

namespace Aplicacion.Comentarios
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid ComentarioId { get; set; }
            public string Alumno { get; set; }
            public int? Puntaje { get; set; }
            public string ComentarioTexto { get; set; }
            public Guid? CursoId { get; set; }
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
                var comentario = await _comentarioRepository.Get(request.ComentarioId);

                if (comentario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "EL Comentario no existe" });
                }

                var comentarioModel = new ComentarioModel
                {
                    Alumno = request.Alumno ?? comentario.Alumno,
                    Puntaje = request.Puntaje ?? comentario.Puntaje,
                    ComentarioTexto = request.ComentarioTexto ?? comentario.ComentarioTexto,
                    CursoId = request.CursoId ?? comentario.CursoId
                };

                var resultado = await _comentarioRepository.Post(comentarioModel);

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se puedo actualizar el isntructor");
            }
        }
    }
}