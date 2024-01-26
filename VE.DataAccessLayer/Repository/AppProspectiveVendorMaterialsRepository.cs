using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppProspectiveVendorMaterialsRepository : DbConnection, IRepository<AppProspectiveVendorMaterials>
    {
        public async Task<IEnumerable<AppProspectiveVendorMaterials>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendorMaterials";
            return await LoadData<AppProspectiveVendorMaterials, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(AppProspectiveVendorMaterials data)
        {
            const string sqlQuery =
                @"INSERT INTO [VE].[AppProspectiveVendorMaterials] ([ProspectiveVendorId],[MaterialCode],[ExtraProperties],[ConcurrencyStamp],[CreationTime],[CreatorId],[LastModificationTime],[LastModifierId],[IsDeleted],[DeleterId],[DeletionTime],[VendorCode]) 
                                       VALUES (@ProspectiveVendorId, @MaterialCode, @ExtraProperties, @ConcurrencyStamp, @CreationTime, @CreatorId, @LastModificationTime, @LastModifierId, @IsDeleted, @DeleterId, @DeletionTime,@VendorCode)";

            return await SaveData(sqlQuery, data);
        }
    }
}