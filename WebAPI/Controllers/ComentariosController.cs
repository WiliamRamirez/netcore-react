using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Comentarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Comentario;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
public class ComentariosController : MiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ComentarioModel>>> Get()
        {
            return await Mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComentarioModel>> Get(Guid id)
        {
            return await Mediator.Send(new ConsultaId.Ejecuta { ComentarioId = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put(Guid id, Editar.Ejecuta data)
        {
            data.ComentarioId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { ComentarioId = id });
        }
    }
}