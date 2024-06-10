using Microsoft.Extensions.Configuration;
using PJENL.API.CleanArchitecture.ApplicationCore.Common.Abstractions.Data;
using System.Data;
using System.Data.SqlClient;

namespace PJENL.API.CleanArchitecture.ApplicationCore.Infrastructure.Persistence
{
    public class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
    {
        /// <summary>
        /// Este metodo crea una instancia de SqlConection en base al nombre de la cadena de conexion.
        /// </summary>
        /// <param name="connectionName">Nombre de la cadena de conexión</param>
        /// <returns></returns>
        public IDbConnection CreateConnection(string connectionName)
        {
            var connectionString = configuration.GetConnectionString(connectionName) ?? throw new ApplicationException($"Connection string for '{connectionName}' not found.");
            return new SqlConnection(connectionString);
        }
    }
}
