using System.Threading.Tasks;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppProspectiveVendorRepository : IRepository<AppProspectiveVendors>
    {
        Task<AppProspectiveVendors> GetByCode(string code);
        Task<int> UpdateStatus(string code, Status status, int PendingWithUserId);
    }
}