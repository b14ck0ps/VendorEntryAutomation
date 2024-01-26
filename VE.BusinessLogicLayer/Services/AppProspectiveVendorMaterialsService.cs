using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.BusinessLogicLayer.Services
{
    public class AppProspectiveVendorMaterialsService
    {
        private readonly IAppProspectiveVendorMaterialsRepository _appProspectiveVendorMaterialsRepository;

        public AppProspectiveVendorMaterialsService()
        {
            _appProspectiveVendorMaterialsRepository = Factory.GetAppProspectiveVendorMaterialsRepository();
        }

        public async Task<IEnumerable<AppProspectiveVendorMaterials>> GetAll()
        {
            return await _appProspectiveVendorMaterialsRepository.GetAll();
        }

        public async Task<int> Insert(AppProspectiveVendorMaterials data)
        {
            return await _appProspectiveVendorMaterialsRepository.Insert(data);
        }

        public async Task<IEnumerable<AppProspectiveVendorMaterials>> GetByCode(string code)
        {
            return await _appProspectiveVendorMaterialsRepository.GetByCode(code);
        }
    }
}