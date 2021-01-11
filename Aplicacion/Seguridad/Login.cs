using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Aplicacion.Contratos;
using Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                this._context = context;
                this._userManager = userManager;
                this._signInManager = signInManager;
                this._jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);

                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                var resultRoles = await _userManager.GetRolesAsync(usuario);

                var listRoles = new List<string>(resultRoles);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(usuario.Id)).FirstOrDefaultAsync();

                if (resultado.Succeeded)
                {
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
                            Token = _jwtGenerador.CrearToken(usuario, listRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = imagenCliente
                        };
                    }
                    else
                    {
                        return new UsuarioData
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            Token = _jwtGenerador.CrearToken(usuario, listRoles),
                            Username = usuario.UserName,
                            Email = usuario.Email,
                            ImagenPerfil = null
                        };

                    }

                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}