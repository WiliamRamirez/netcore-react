using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);

                if (curso == null)
                {
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { curso = "No se encontro el curso" });
                }

                // Eliminar comentario del curso de la Tabla comentario
                var comentariosEntitie = await _context.Comentario.Where(x => x.CursoId == curso.CursoId).ToListAsync();

                if (comentariosEntitie != null)
                {
                    foreach (var comentarioEntite in comentariosEntitie)
                    {
                        _context.Comentario.Remove(comentarioEntite);
                    }
                }



                // Eliminar precio del curso de la TABLA Precio
                var precioEntitie = await _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefaultAsync();

                if (precioEntitie != null)
                {
                    _context.Precio.Remove(precioEntitie);
                }
                

                // Se elimina intructores de la tabla CursoInstructor
                var instructoresDb = await _context.CursoInstructor.Where(x => x.CursoId == request.Id).ToListAsync();

                foreach (var instructorEliminarDb in instructoresDb)
                {
                    _context.CursoInstructor.Remove(instructorEliminarDb);
                }


                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}