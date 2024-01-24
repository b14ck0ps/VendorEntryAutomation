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
    public class VendorEnlistmentLogService
    {
        private readonly IRepository<VendorEnlistmentLog> _vendorEnlistmentLogRepository;

        public VendorEnlistmentLogService()
        {
            _vendorEnlistmentLogRepository = Factory.GetVendorEnlistmentLogRepository();
        }

        public async Task<IEnumerable<VendorEnlistmentLog>> GetAll()
        {
            return await _vendorEnlistmentLogRepository.GetAll();
        }

        public async Task<int> Insert(VendorEnlistmentLog data)
        {
            return await _vendorEnlistmentLogRepository.Insert(data);
        }
    }
}
