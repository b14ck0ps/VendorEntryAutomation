using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppVendorEnlistmentLogsRepository:IRepository<AppVendorEnlistmentLogs>
    {
        Task<IEnumerable<AppVendorEnlistmentLogs>> GetByCode(string code);
    }
}