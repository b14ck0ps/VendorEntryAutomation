using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.BusinessLogicLayer.Services
{
    public class AppProspectiveVendorsService
    {
        private readonly IRepository<AppProspectiveVendors> _appProspectiveVendorsRepository;

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
    }
}