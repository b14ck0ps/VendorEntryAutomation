using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppProspectiveVendorsRepository : DbConnection, IRepository<AppProspectiveVendors>
    {
        public async Task<IEnumerable<AppProspectiveVendors>> GetAll()
        {
            const string sqlQuery = "SELECT * FROM [VE].AppProspectiveVendors";
            return await LoadData<AppProspectiveVendors, dynamic>(sqlQuery, new { });
        }

        public async Task<int> Insert(AppProspectiveVendors data)
        {
            const string sqlQuery = @"INSERT INTO [VE].[AppProspectiveVendors] ([Code],[RequestorID],[ServiceDescription],[RequirementGeneral],[RequirementOther],[TypeOfSupplierId],[ExisitngSupplierCount],[ExisitngSupplierProblem],[NewSupplierAdditionReason],[VendorName],[VendorEmail],[Status],[ExtraProperties],[ConcurrencyStamp],[CreationTime],[CreatorId],[LastModificationTime],[LastModifierId],[IsDeleted],[DeleterId],[DeletionTime],[PendingWithUserId],[IsIncludedIntoSAP],[SAPVendorCode],[ScoreCard]) 
                                       VALUES (@Code, @RequestorID, @ServiceDescription, @RequirementGeneral, @RequirementOther, @TypeOfSupplierId, @ExisitngSupplierCount, @ExisitngSupplierProblem, @NewSupplierAdditionReason, @VendorName, @VendorEmail, @Status, @ExtraProperties, @ConcurrencyStamp, @CreationTime, @CreatorId, @LastModificationTime, @LastModifierId, @IsDeleted, @DeleterId, @DeletionTime, @PendingWithUserId, @IsIncludedIntoSAP, @SAPVendorCode, @ScoreCard)";


            return await SaveData(sqlQuery, data);
        }
    }
}
