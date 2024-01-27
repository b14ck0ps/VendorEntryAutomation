using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class VendorEnlistmentLogRepository : DbConnection, IRepository<VendorEnlistmentLog>
    {
        public async Task<IEnumerable<VendorEnlistmentLog>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].VendorEnlistmentLog";
            return await LoadData<VendorEnlistmentLog, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(VendorEnlistmentLog data)
        {
            const string sqlQuery = @"INSERT INTO [VE].[VendorEnlistmentLog]
           ([VendorCode],[Status],[ActionBy],[Comment],[CreatedBy],[CreatedDate]
           ,[UpdatedBy],[UpdatedDate])
             VALUES
           (@VendorCode,@Status,@ActionBy,@Comment,@CreatedBy,@CreatedDate,@UpdatedBy,
           @UpdatedDate)";

            return await SaveData(sqlQuery, data);
        }
    }
}