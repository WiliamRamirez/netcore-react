using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Comentario
{
    public interface IComentario
    {
        Task<IEnumerable<ComentarioModel>> Get();

        Task<ComentarioModel> Get(Guid id);

        Task<int> Post(ComentarioModel parametros);

        Task<int> Put(ComentarioModel parametros);

        Task<int> Delete(Guid id);
    }
}