using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppVendorEnlistmentLogsRepository : DbConnection,IAppVendorEnlistmentLogsRepository
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

        public async Task<IEnumerable<AppVendorEnlistmentLogs>> GetByCode(string code)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppVendorEnlistmentLogs WHERE Code = @Code";
            return await LoadData<AppVendorEnlistmentLogs, dynamic>(sqlQuery, new { Code = code });
        }
    }
}