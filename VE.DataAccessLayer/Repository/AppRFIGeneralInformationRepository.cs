using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppRFIGeneralInformationRepository : DbConnection, IAppRFIGeneralInformationRepository
    {
        public Task<IEnumerable<AppRFIGeneralInformations>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AppRFIGeneralInformations> GetById(int ProspectiveVendorId)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppRFIGeneralInformations WHERE ProspectiveVendorId = @ProspectiveVendorId";
            return (await LoadData<AppRFIGeneralInformations, dynamic>(sqlQuery, new { ProspectiveVendorId = ProspectiveVendorId })).FirstOrDefault();
        }

        public Task<int> Insert(AppRFIGeneralInformations data)
        {
            throw new NotImplementedException();
        }
    }
}
