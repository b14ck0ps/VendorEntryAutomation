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
    public class AppVendorEnlistmentLogsService
    {
        private readonly IRepository<AppVendorEnlistmentLogs> _appVendorEnlistmentLogsRepository;

        public AppVendorEnlistmentLogsService()
        {
            _appVendorEnlistmentLogsRepository = Factory.GetAppVendorEnlistmentLogsRepository();
        }

        public async Task<IEnumerable<AppVendorEnlistmentLogs>> GetAll()
        {
            return await _appVendorEnlistmentLogsRepository.GetAll();
        }

        public async Task<int> Insert(AppVendorEnlistmentLogs data)
        {
            return await _appVendorEnlistmentLogsRepository.Insert(data);
        }
    }
}
