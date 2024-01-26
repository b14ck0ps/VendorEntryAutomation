using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.Entities;

namespace VE.BusinessLogicLayer.Services
{
    public class VendorEnlistmentService
    {
        private readonly IRepository<VendorEnlistment> _vendorEnlistmentRepository;

        public VendorEnlistmentService()
        {
            _vendorEnlistmentRepository = Factory.GetVendorEnlistmentRepository();
        }

        public async Task<IEnumerable<VendorEnlistment>> GetAll()
        {
            return await _vendorEnlistmentRepository.GetAll();
        }

        public async Task<int> Insert(VendorEnlistment data)
        {
            return await _vendorEnlistmentRepository.Insert(data);
        }
    }
}