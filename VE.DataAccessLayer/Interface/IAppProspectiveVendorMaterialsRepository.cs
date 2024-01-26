using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppProspectiveVendorMaterialsRepository : IRepository<AppProspectiveVendorMaterials>
    {
        Task<IEnumerable<AppProspectiveVendorMaterials>> GetByCode(string code);
    }
}