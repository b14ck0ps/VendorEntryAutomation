using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer.Interface
{
    public interface IAppRFILegalEstablishmentRepository : IRepository<AppRFILegalEstablishments>
    {
        Task<AppRFILegalEstablishments> GetById(int ProspectiveVendorId);
    }
}
