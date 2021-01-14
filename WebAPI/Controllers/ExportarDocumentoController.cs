using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Cursos;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class ExportarDocumentoController : MiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<Stream>> GetTask()
        {
            return await Mediator.Send(new ExportPDF.Consulta());
        }
    }
}