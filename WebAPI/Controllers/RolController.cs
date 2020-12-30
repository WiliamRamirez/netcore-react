using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RolController : MiControllerBase
    {

        [HttpGet("lista")]
        public async Task<ActionResult<List<IdentityRole>>> Get()
        {
            return await Mediator.Send( new RolLista.Ejecuta() );
        }

        [HttpGet("listarRoles/{UserName}")]
        public async Task<ActionResult<List<string>>> Get(string UserName)
        {

            return await Mediator.Send(new ObtenerRolPorUsuario.Ejecuta{ UserName = UserName  });
        }

        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Post(RolNuevo.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpPost("agregarRolUsuario")]
        public async Task<ActionResult<Unit>> Post(UsuarioRolAgregar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpDelete("DeleteRolUsuario")]
        public async Task<ActionResult<Unit>> Delete(UsuarioRolEliminar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Delete(RolEliminar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    }
}