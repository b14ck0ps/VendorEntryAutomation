using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppRFIExperienceProductAvailabilitiesRepository : IRepository<AppRFIExperienceProductAvailabilities>
    {
        Task<AppRFIExperienceProductAvailabilities> GetById(int ProspectiveVendorId);
    }
}
