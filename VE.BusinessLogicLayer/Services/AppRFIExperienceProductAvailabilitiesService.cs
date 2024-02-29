using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.BusinessLogicLayer.Services
{

    public class AppRFIExperienceProductAvailabilitiesService
    {
        private readonly IAppRFIExperienceProductAvailabilitiesRepository _appRFIExperienceProductAvailabilitiesRepository;

        public AppRFIExperienceProductAvailabilitiesService()
        {
            _appRFIExperienceProductAvailabilitiesRepository = Factory.GetAppRFIExperienceProductAvailabilitiesRepository();
        }

        public async Task<AppRFIExperienceProductAvailabilities> GetById(int ProspectiveVendorId)
        {
            return await _appRFIExperienceProductAvailabilitiesRepository.GetById(ProspectiveVendorId);
        }
    }
}
