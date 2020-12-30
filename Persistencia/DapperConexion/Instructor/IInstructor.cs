using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> Get();

        Task<InstructorModel> Get(Guid id);

        Task<int> Post(InstructorModel parametros);

        Task<int> Put(InstructorModel parametros);
        
        Task<int> Delete(Guid id);
    }
}