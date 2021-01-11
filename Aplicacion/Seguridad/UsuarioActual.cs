using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData>
        {

        }

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                this._context = context;
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
                this._usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

                var resultRoles = await _userManager.GetRolesAsync(usuario);

                var listRoles = new List<string>(resultRoles);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                if (imagenPerfil != null)
                {
                    var imagenCliente = new ImagenDTO
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };

                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Username = usuario.UserName,
                        Token = _jwtGenerador.CrearToken(usuario, listRoles),
                        Email = usuario.Email,
                        ImagenPerfil = imagenCliente

                    };
                }

                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Username = usuario.UserName,
                    Token = _jwtGenerador.CrearToken(usuario, listRoles),
                    Email = usuario.Email,
                    ImagenPerfil = null
                };


            }
        }


    }
}