using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Comentario;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid ComentarioId { get; set; }
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

                var resultado = await _comentarioRepository.Delete(request.ComentarioId);
                
                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el instructor");
            }
        }
    }
}