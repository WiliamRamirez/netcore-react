using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructoresController : MiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> Get()
        {
            return await Mediator.Send(new Consulta.ListaInstructor());
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorModel>> Get(Guid id)
        {
            return await Mediator.Send(new ConsultaId.Ejecuta{ InstructorId = id });
        }


        [HttpPost]
        public async Task<ActionResult<Unit>> Post(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Put(Guid id, Editar.Ejecuta data)
        {
            data.InstructorId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Ejecuta{InstructorId = id});
        }
        
    }
}