using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class VendorEnlistmentRepository : DbConnection, IRepository<VendorEnlistment>
    {
        public async Task<IEnumerable<VendorEnlistment>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].VendorEnlistment";
            return await LoadData<VendorEnlistment, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(VendorEnlistment data)
        {
            const string sqlQuery = @"INSERT INTO [VE].[VendorEnlistment]
           ([Name],[Email],[ProductMaterial],[Description],[SupGenralReq],[SupOtherReq],[SupType],[ExistingSupCount]
           ,[ExistingSupProblem],[SupAddReason],[Status],[PendingWith],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate]
           ,[VendorCode])
     VALUES
           (@Name,@Email,@ProductMaterial,@Description,@SupGenralReq,@SupOtherReq
           ,@SupType,@ExistingSupCount,@ExistingSupProblem,@SupAddReason
           ,@Status,@PendingWith,@CreatedBy,@CreatedDate,@UpdatedBy,@UpdatedDate,@VendorCode)";


            return await SaveData(sqlQuery, data);
        }
    }
}