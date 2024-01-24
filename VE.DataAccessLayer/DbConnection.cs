using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using VE.DataAccessLayer.Interface;

namespace VE.DataAccessLayer
{
    public class DbConnection : ISqlDataAccess
    {
public string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        public async Task<IEnumerable<T>> LoadData<T, U>(string sqlQuery, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return await connection.QueryAsync<T>(sqlQuery, parameters, commandType: CommandType.Text);
            }
        }

        public async Task<int> SaveData<T>(string sqlQuery, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return await connection.ExecuteAsync(sqlQuery, parameters, commandType: CommandType.Text);
            }
        }
    }
}