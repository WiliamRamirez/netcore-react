using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConexion _factoryConnection;

        public InstructorRepositorio(IFactoryConexion factoryConnection )
        {
            this._factoryConnection = factoryConnection;
        }

        public async Task<int> Delete(Guid id)
        {
            var storeProcedure = "usp_Delete_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    InstructorId = id
                },
                commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo eliminar el nuevo instructor", e);
            }
        }

        public async Task<IEnumerable<InstructorModel>> Get()
        {
            IEnumerable<InstructorModel> instructorList = null;

            var storeProcedure = "usp_Get_Instructores";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure,null, commandType : CommandType.StoredProcedure);
            }
            catch (Exception e)
            {       
                throw new Exception("error en la consulta de datos", e) ;
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return instructorList;
        }

        public async Task<InstructorModel> Get(Guid id)
        {
            InstructorModel instructor = null;

            var storeProcedure = "usp_GetId_Instructores";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructor = await connection.QueryFirstOrDefaultAsync<InstructorModel>(storeProcedure,
                new
                {
                    InstructorId = id
                },
                commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }

            return instructor;

        }

        public async Task<int> Post(InstructorModel parametros)
        {
            var storeProcedure = "usp_Post_Instructores";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = parametros.Nombre,
                    Apellidos = parametros.Apellidos,
                    Titulo = parametros.Titulo,
                },
                commandType : CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception( "No se pudo gurdar el nuevo instructor", e );
            }

        }

        public async Task<int> Put(InstructorModel parametros)
        {
            var storeProcedure = "usp_Put_Instructores";
            
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    InstructorId = parametros.InstructorId,
                    Nombre = parametros.Nombre,
                    Apellidos = parametros.Apellidos,
                    Titulo = parametros.Titulo,
                },
                commandType: CommandType.StoredProcedure);

                _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (System.Exception e)
            {
                throw new Exception("No se pudo editar el nuevo instructor", e);
            }
        }
    }
}