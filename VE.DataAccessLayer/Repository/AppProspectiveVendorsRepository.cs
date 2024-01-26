using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;

namespace VE.DataAccessLayer.Repository
{
    public class AppProspectiveVendorsRepository : DbConnection, IAppProspectiveVendorRepository
    {
        public async Task<IEnumerable<AppProspectiveVendors>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendors";
            return await LoadData<AppProspectiveVendors, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(AppProspectiveVendors data)
        {
            const string sqlQuery =
                @"INSERT INTO [VE].[AppProspectiveVendors] ([Code],[RequestorID],[ServiceDescription],[RequirementGeneral],[RequirementOther],[TypeOfSupplierId],[ExisitngSupplierCount],[ExisitngSupplierProblem],[NewSupplierAdditionReason],[VendorName],[VendorEmail],[Status],[ExtraProperties],[ConcurrencyStamp],[CreationTime],[CreatorId],[LastModificationTime],[LastModifierId],[IsDeleted],[DeleterId],[DeletionTime],[PendingWithUserId],[IsIncludedIntoSAP],[SAPVendorCode],[ScoreCard]) 
                                       VALUES (@Code, @RequestorID, @ServiceDescription, @RequirementGeneral, @RequirementOther, @TypeOfSupplierId, @ExisitngSupplierCount, @ExisitngSupplierProblem, @NewSupplierAdditionReason, @VendorName, @VendorEmail, @Status, @ExtraProperties, @ConcurrencyStamp, @CreationTime, @CreatorId, @LastModificationTime, @LastModifierId, @IsDeleted, @DeleterId, @DeletionTime, @PendingWithUserId, @IsIncludedIntoSAP, @SAPVendorCode, @ScoreCard)";


            return await SaveData(sqlQuery, data);
        }
        public async Task<AppProspectiveVendors> GetByCode(string code)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendors WHERE Code = @Code";
            return (await LoadData<AppProspectiveVendors, dynamic>(sqlQuery, new { Code = code })).FirstOrDefault();
        }

        public async Task<int> UpdateStatus(string code, Status status)
        {
            const string sqlQuery = "UPDATE [VE].[AppProspectiveVendors] SET Status = @Status WHERE Code = @Code";
            return await SaveData(sqlQuery, new { Code = code, Status = status });
        }
    }
}