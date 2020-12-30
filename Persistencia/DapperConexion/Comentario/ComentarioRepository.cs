using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Comentario
{
    public class ComentarioRepository : IComentario
    {
        private readonly IFactoryConexion _factoryConnection;

        public ComentarioRepository(IFactoryConexion factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }

        public async Task<int> Delete(Guid id)
        {
            var storeProcedure = "usp_Delete_Comentarios";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure,
                new
                {
                    ComentarioId = id
                },
                commandType: CommandType.StoredProcedure);

                return resultado;

            }
            catch (System.Exception e)
            {

                throw new Exception("Error en la base de datos", e);
            }
        }

        public async Task<IEnumerable<ComentarioModel>> Get()
        {
            IEnumerable<ComentarioModel> comentariosList = null;

            var storeProcedure = "usp_Get_Comentarios";

            try
            {
                var connection = _factoryConnection.GetConnection();
                comentariosList = await connection.QueryAsync<ComentarioModel>(storeProcedure,
                null, commandType: CommandType.StoredProcedure);
                
            }
            catch (Exception e)
            {
                throw new Exception("error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return comentariosList;
        }

        public async Task<ComentarioModel> Get(Guid id)
        {
            var storeProcedure = "usp_GetId_Comentarios";

            ComentarioModel comentario = null;

            try
            {
                var connection = _factoryConnection.GetConnection();
                comentario = await connection.QueryFirstOrDefaultAsync<ComentarioModel>(storeProcedure,
                new
                {
                    ComentarioId = id
                },
                commandType: CommandType.StoredProcedure );


            }
            catch (System.Exception e)
            {
                
                throw new Exception("No se pudo hacer la consulta a la BD", e) ;
            }

            finally
            {
                _factoryConnection.CloseConnection();
            }

            return comentario;

        }

        public async Task<int> Post(ComentarioModel parametros)
        {
            var storeProcedure = "usp_Post_Comentarios";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure,
                new
                {
                    ComentarioId = Guid.NewGuid(),
                    Alumno = parametros.Alumno,
                    Puntaje = parametros.Puntaje,
                    ComentarioTexto = parametros.ComentarioTexto,
                    CursoId = parametros.CursoId
                },
                commandType: CommandType.StoredProcedure);
                
                _factoryConnection.CloseConnection();

                return resultado;

            }
            catch (Exception e)
            {
                throw new Exception("error en la consulta de datos", e);
            }
        }

        public Task<int> Put(ComentarioModel parametros)
        {
            var storeProcedure = "usp_Put_Comentarios";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = connection.ExecuteAsync(storeProcedure,
                new
                {
                    ComentarioId = parametros.ComentarioId,
                    Alumno = parametros.Alumno,
                    Puntaje = parametros.Puntaje,
                    ComentarioTexto = parametros.ComentarioTexto,
                    CursoId = parametros.CursoId
                },

                commandType: CommandType.StoredProcedure);

                return resultado;
            }
            catch (System.Exception e)
            {

                throw new Exception("error en la actualizacion de datos en la BD", e);
            }
            
        }
    }
}