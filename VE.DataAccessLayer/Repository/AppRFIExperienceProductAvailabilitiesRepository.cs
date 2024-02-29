using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Repository
{
    public class AppRFIExperienceProductAvailabilitiesRepository : DbConnection, IAppRFIExperienceProductAvailabilitiesRepository
    {
        public Task<IEnumerable<AppRFIExperienceProductAvailabilities>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AppRFIExperienceProductAvailabilities> GetById(int ProspectiveVendorId)
        {
            const string sqlQuery = "SELECT * FROM [VE].AppRFIExperienceProductAvailabilities WHERE ProspectiveVendorId = @ProspectiveVendorId";
            return (await LoadData<AppRFIExperienceProductAvailabilities, dynamic>(sqlQuery, new { ProspectiveVendorId = ProspectiveVendorId })).FirstOrDefault();
        }

        public Task<int> Insert(AppRFIExperienceProductAvailabilities data)
        {
            throw new NotImplementedException();
        }
    }
}
