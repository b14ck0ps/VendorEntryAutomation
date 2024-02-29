using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppRFILegalEstablishmentRepository : DbConnection, IAppRFILegalEstablishmentRepository
    {
        public Task<IEnumerable<AppRFILegalEstablishments>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AppRFILegalEstablishments> GetById(int ProspectiveVendorId)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppRFILegalEstablishments WHERE ProspectiveVendorId = @ProspectiveVendorId";
            return (await LoadData<AppRFILegalEstablishments, dynamic>(sqlQuery, new { ProspectiveVendorId = ProspectiveVendorId })).FirstOrDefault();
        }

        public Task<int> Insert(AppRFILegalEstablishments data)
        {
            throw new NotImplementedException();
        }
    }
}
