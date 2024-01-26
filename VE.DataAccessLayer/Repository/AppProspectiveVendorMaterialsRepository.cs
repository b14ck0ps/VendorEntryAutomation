using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppProspectiveVendorMaterialsRepository : DbConnection, IAppProspectiveVendorMaterialsRepository
    {
        public async Task<IEnumerable<AppProspectiveVendorMaterials>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendorMaterials";
            return await LoadData<AppProspectiveVendorMaterials, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(AppProspectiveVendorMaterials data)
        {
            const string sqlQuery =
                @"INSERT INTO [VE].[AppProspectiveVendorMaterials] ([ProspectiveVendorId],[MaterialCode],[ExtraProperties],[ConcurrencyStamp],[CreationTime],[CreatorId],[LastModificationTime],[LastModifierId],[IsDeleted],[DeleterId],[DeletionTime],[VendorCode],[MaterialName]) 
                                       VALUES (@ProspectiveVendorId, @MaterialCode, @ExtraProperties, @ConcurrencyStamp, @CreationTime, @CreatorId, @LastModificationTime, @LastModifierId, @IsDeleted, @DeleterId, @DeletionTime,@VendorCode,@MaterialName)";

            return await SaveData(sqlQuery, data);
        }

        public async Task<IEnumerable<AppProspectiveVendorMaterials>> GetByCode(string code)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendorMaterials WHERE VendorCode = @Code";
            return await LoadData<AppProspectiveVendorMaterials, dynamic>(sqlQuery, new { Code = code });
        }
    }
}