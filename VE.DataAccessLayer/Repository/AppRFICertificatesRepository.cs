using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppRFICertificatesRepository : DbConnection, IAppRFICertificatesRepository
    {
        public Task<IEnumerable<AppRFICertificates>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AppRFICertificates> GetById(int ProspectiveVendorId)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppRFICertificates WHERE ProspectiveVendorId = @ProspectiveVendorId";
            return (await LoadData<AppRFICertificates, dynamic>(sqlQuery, new { ProspectiveVendorId = ProspectiveVendorId })).FirstOrDefault();
        }

        public Task<int> Insert(AppRFICertificates data)
        {
            throw new NotImplementedException();
        }
    }
}
