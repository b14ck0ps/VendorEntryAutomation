using System.Threading.Tasks;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppProspectiveVendorRepository : IRepository<AppProspectiveVendors>
    {
        Task<AppProspectiveVendors> GetByCode(string code);
    }
}