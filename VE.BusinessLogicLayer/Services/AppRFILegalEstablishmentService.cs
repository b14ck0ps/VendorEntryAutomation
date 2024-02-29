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
    public class AppRFILegalEstablishmentService
    {
        private readonly IAppRFILegalEstablishmentRepository _appRFILegalEstablishmentRepository;

        public AppRFILegalEstablishmentService()
        {
            _appRFILegalEstablishmentRepository = Factory.GetAppRFILegalEstablishmentRepository();
        }

        public async Task<AppRFILegalEstablishments> GetById(int ProspectiveVendorId)
        {
            return await _appRFILegalEstablishmentRepository.GetById(ProspectiveVendorId);
        }
    }
}
