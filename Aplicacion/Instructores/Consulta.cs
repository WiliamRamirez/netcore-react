using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Consulta
    {
        public class ListaInstructor : IRequest<List<InstructorModel>>
        {

        }

        public class Handler : IRequestHandler<ListaInstructor, List<InstructorModel>>
        {
            private readonly IInstructor _instructorRepository;

            public Handler(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }
            public async Task<List<InstructorModel>> Handle(ListaInstructor request, CancellationToken cancellationToken)
            {
                var resultado = await _instructorRepository.Get();
                return resultado.ToList();
            }
        }
    }
}