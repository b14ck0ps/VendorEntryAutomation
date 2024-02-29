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
    public class AppRFIGeneralInformationService 
    {
        private readonly IAppRFIGeneralInformationRepository _appRFIGeneralInformationRepository;

        public AppRFIGeneralInformationService()
        {
            _appRFIGeneralInformationRepository = Factory.GetAppRFIGeneralInformationRepository();
        }

        public async Task<AppRFIGeneralInformations> GetById(int ProspectiveVendorId)
        {
            return await _appRFIGeneralInformationRepository.GetById(ProspectiveVendorId);
        }
    }
}
