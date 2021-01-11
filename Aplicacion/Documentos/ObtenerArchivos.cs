using System.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Documentos
{
    public class ObtenerArchivos
    {
        public class Ejecuta : IRequest<ArchivoDTO>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta, ArchivoDTO>
        {
            private readonly CursosOnlineContext _context;

            public Handler(CursosOnlineContext context)
            {
                this._context = context;
            }
            public async Task<ArchivoDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var archivo = await _context.Documento.Where(x => x.ObjetoReferencia == request.Id).FirstOrDefaultAsync();

                if (archivo == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro la imagen"});
                }

                var archivoDto = new ArchivoDTO
                {
                    Data = Convert.ToBase64String(archivo.Contenido),
                    Nombre = archivo.Nombre,
                    Extension = archivo.Extension
                };

                return archivoDto;
            }
        }


    }
}