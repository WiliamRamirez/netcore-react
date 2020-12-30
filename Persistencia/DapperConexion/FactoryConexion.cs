using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Persistencia.DapperConexion
{
    public class FactoryConexion : IFactoryConexion
    {
        private IDbConnection _connection;
        private readonly IOptions<ConexionConfiguracion> _config;

        public FactoryConexion(IOptions<ConexionConfiguracion> config)
        {
            this._config = config;
        } 


        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_config.Value.DefaultConnection);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;

        }
    }
}