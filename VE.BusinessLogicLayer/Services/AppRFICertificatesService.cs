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
    public class AppRFICertificatesService
    {
        private readonly IAppRFICertificatesRepository _appRFICertificatesRepository;

        public AppRFICertificatesService()
        {
            _appRFICertificatesRepository = Factory.GetAppRFICertificatesRepository();
        }

        public async Task<AppRFICertificates> GetById(int ProspectiveVendorId)
        {
            return await _appRFICertificatesRepository.GetById(ProspectiveVendorId);
        }
    }
}
