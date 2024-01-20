using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.DbTable;

namespace VE.DataAccessLayer.Repository
{
    public class TestTableRepository : DbConnection, IRepository<TestTable>
    {
        

        public async Task<IEnumerable<TestTable>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].TestTable";
            return await LoadData<TestTable, dynamic>(sqlQuery, new {});
        }

        public async Task<int> Insert(TestTable data)
        {
            const string sqlQuery = "INSERT INTO [VE].TestTable (Name, PendingWith) VALUES (@Name, @PendingWith)";
            return await SaveData(sqlQuery, data);
        }
    }

}
