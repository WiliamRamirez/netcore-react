using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest 
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set; }

            public List<Guid> ListaInstructor { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.Precio).NotEmpty();
                RuleFor(x => x.Promocion).NotEmpty();
            }
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
                Guid _cursoId = Guid.NewGuid();

                // Agregar precio
                
                var curso = new Curso
                {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow
                };

                _contex.Curso.Add(curso);

                if (request.ListaInstructor != null)
                {
                    
                    
                    foreach (var id in request.ListaInstructor)
                    {
                        var _cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };

                        _contex.CursoInstructor.Add(_cursoInstructor);
                    }
                }

                // Agregar precio

                var _precioId = Guid.NewGuid();

                var precio = new Precio
                {
                    PrecioId = _precioId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    CursoId = curso.CursoId
                };

                _contex.Precio.Add(precio);

                var valor = await _contex.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");


            }
        }
    }
}