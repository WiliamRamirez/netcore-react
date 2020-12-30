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
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }

        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
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

                userIden.NombreCompleto = request.Nombre + " " + request.Apellidos;
                userIden.Email = request.Email;
                userIden.PasswordHash = _passwordHasher.HashPassword(userIden, request.Password);

                var resultUpdate = await _userManager.UpdateAsync(userIden);

                var resultRoles = await _userManager.GetRolesAsync(userIden);

                var listRoles = new List<string>(resultRoles);


                if (resultUpdate.Succeeded)
                {
                    return new UsuarioData
                    {

                        NombreCompleto = userIden.NombreCompleto,
                        Username = userIden.UserName,
                        Email = userIden.Email,
                        Token = _jwtGenerador.CrearToken(userIden, listRoles)

                    };
                }


                throw new Exception("No se pudo actualizar el usuario");
            }
        }
    }
}