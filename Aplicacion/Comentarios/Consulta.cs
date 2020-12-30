using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using MediatR;
using Persistencia.DapperConexion.Comentario;

namespace Aplicacion.Comentarios
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<ComentarioModel>>
        {
            public string Alumno { get; set; }
            public int Puntaje { get; set; }
            public string ComentarioTexto { get; set; }
            public Guid CursoId { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta, List<ComentarioModel>>
        {
            private readonly IComentario _comentarioRepository;

            public Handler(IComentario comentarioRepository)
            {
                this._comentarioRepository = comentarioRepository;
            }
            
            public async Task<List<ComentarioModel>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await _comentarioRepository.Get();

                return resultado.ToList();
            }
        }
    }
}