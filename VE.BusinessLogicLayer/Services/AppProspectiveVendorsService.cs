using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.BusinessLogicLayer.Services
{
    public class AppProspectiveVendorsService
    {
        private readonly IAppProspectiveVendorRepository _appProspectiveVendorsRepository;

        public AppProspectiveVendorsService()
        {
            _appProspectiveVendorsRepository = Factory.GetAppProspectiveVendorsRepository();
        }

        public async Task<IEnumerable<AppProspectiveVendors>> GetAll()
        {
            return await _appProspectiveVendorsRepository.GetAll();
        }

        public async Task<int> Insert(AppProspectiveVendors data)
        {
            return await _appProspectiveVendorsRepository.Insert(data);
        }

        public async Task<AppProspectiveVendors> GetByCode(string code)
        {
            return await _appProspectiveVendorsRepository.GetByCode(code);
        }
    }
}