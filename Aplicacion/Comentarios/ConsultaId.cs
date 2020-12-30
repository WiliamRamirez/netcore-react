using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Comentario;

namespace Aplicacion.Comentarios
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<ComentarioModel>
        {
            public Guid ComentarioId { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta, ComentarioModel>
        {
            private readonly IComentario _comentarioRepository;

            public Handler(IComentario comentarioRepository)
            {
                this._comentarioRepository = comentarioRepository;
            }
            public async Task<ComentarioModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = await _comentarioRepository.Get(request.ComentarioId);

                if (comentario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "EL Comentario no existe" });
                }

                return comentario;
            }
        }
    }
}