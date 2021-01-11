using System;
using System.Threading.Tasks;
using Aplicacion.Documentos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class DocumentoController : MiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ArchivoDTO>> Get(Guid id)
        {
            return await Mediator.Send(new ObtenerArchivos.Ejecuta { Id = id }); 
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(SubirArchivo.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

    }
}