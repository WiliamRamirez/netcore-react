using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }

            public List<Guid> ListaInstructor { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _contex;
            public Manejador(CursosOnlineContext context)
            {
                _contex = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _contex.Curso.FindAsync(request.CursoId);

                if (curso == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;


                // Editar Precio del curso
                var precioDb = await _contex.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefaultAsync();

                if (precioDb != null)
                {
                    precioDb.PrecioActual = request.Precio ?? precioDb.PrecioActual;
                    precioDb.Promocion = request.Promocion ?? precioDb.Promocion;

                }
                else
                {
                    precioDb = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };

                    _contex.Precio.Add(precioDb);
                }


                if (request.ListaInstructor != null && request.ListaInstructor.Count > 0)
                {
                    // Eliminar Id de isntructuors de la tabla ListaInstructor
                    var instructoresDb = await _contex.CursoInstructor
                        .Where(x => x.CursoId == request.CursoId).ToListAsync();

                    foreach (var instructorEliminar in instructoresDb)
                    {
                        _contex.CursoInstructor.Remove(instructorEliminar);
                    }

                    // Agregar instructores que el cliente ingresa
                    foreach (var instructorId in request.ListaInstructor)
                    {
                        var instructor = new CursoInstructor
                        {
                            CursoId = request.CursoId,
                            InstructorId = instructorId
                        };

                        _contex.CursoInstructor.Add(instructor);

                    }

                }


                var resultado = await _contex.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se Guardaron los cambios en el curso");

            }
        }
    }
}