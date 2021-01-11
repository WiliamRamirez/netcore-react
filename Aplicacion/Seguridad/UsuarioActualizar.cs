using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class UsuarioActualizar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public ImagenDTO ImagenPerfil { get; set; }

        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public Handler(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IPasswordHasher<Usuario> passwordHasher)
            {
                this._context = context;
                this._userManager = userManager;
                this._jwtGenerador = jwtGenerador;
                this._passwordHasher = passwordHasher;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var userIden = await _userManager.FindByNameAsync(request.UserName);

                if (userIden == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No existe el usuario con ese userName" });
                }

                var result = await _context.Users.Where(x => x.Email == request.Email && x.UserName != request.UserName).AnyAsync();

                if (result)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { mensaje = "Este Email Pertenece a otro usuario" });
                }

                var resultadoImagen = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(userIden.Id)).FirstOrDefaultAsync();

                if (request.ImagenPerfil != null)
                {

                    if (resultadoImagen == null)
                    {
                        var imagen = new Documento
                        {
                            Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data),
                            Nombre = request.ImagenPerfil.Nombre,
                            Extension = request.ImagenPerfil.Extension,
                            ObjetoReferencia = new Guid(userIden.Id),
                            DocumentoId = Guid.NewGuid(),
                            FechaCreacion = DateTime.UtcNow
                        };

                        _context.Documento.Add(imagen);
                    }
                    else
                    {
                        resultadoImagen.Contenido = System.Convert.FromBase64String(request.ImagenPerfil.Data) ?? resultadoImagen.Contenido;
                        resultadoImagen.Nombre = request.ImagenPerfil.Nombre ?? resultadoImagen.Nombre;
                        resultadoImagen.Extension = request.ImagenPerfil.Extension ?? resultadoImagen.Extension;
                    }

                }

                userIden.NombreCompleto = request.NombreCompleto;
                userIden.Email = request.Email;
                userIden.PasswordHash = _passwordHasher.HashPassword(userIden, request.Password);

                var resultUpdate = await _userManager.UpdateAsync(userIden);

                var resultRoles = await _userManager.GetRolesAsync(userIden);

                var listRoles = new List<string>(resultRoles);

                var imagenPerfil = await _context.Documento.Where(x => x.ObjetoReferencia == new Guid(userIden.Id)).FirstOrDefaultAsync();

                ImagenDTO imagenCliente = null;

                if (imagenPerfil != null)
                {
                    imagenCliente = new ImagenDTO
                    {
                        Data = Convert.ToBase64String(imagenPerfil.Contenido),
                        Extension = imagenPerfil.Extension,
                        Nombre = imagenPerfil.Nombre
                    };

                }

                if (resultUpdate.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = userIden.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(userIden, listRoles),
                        Username = userIden.UserName,
                        Email = userIden.Email,
                        ImagenPerfil = imagenCliente
                    };
                }

                throw new Exception("No se pudo actualizar el usuario");
            }
        }
    }
}