using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppVendorEnlistmentLogsRepository : DbConnection, IRepository<AppVendorEnlistmentLogs>
    {
        public async Task<IEnumerable<AppVendorEnlistmentLogs>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].AppVendorEnlistmentLogs";
            return await LoadData<AppVendorEnlistmentLogs, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(AppVendorEnlistmentLogs data)
        {
            const string sqlQuery =
                @"INSERT INTO [VE].[AppVendorEnlistmentLogs] ([ProspectiveVendorId],[Code], [Status], [Action], [ActionById], [Comment], [ExtraProperties], [ConcurrencyStamp], [CreationTime], [CreatorId], [LastModificationTime], [LastModifierId]) 
                                      VALUES (@ProspectiveVendorId, @Code, @Status, @Action, @ActionById, @Comment, @ExtraProperties, @ConcurrencyStamp, @CreationTime, @CreatorId, @LastModificationTime, @LastModifierId)";

            return await SaveData(sqlQuery, data);
        }
    }
}