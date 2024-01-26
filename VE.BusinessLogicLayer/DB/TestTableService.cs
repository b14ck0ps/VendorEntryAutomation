using System.Collections.Generic;
using System.Threading.Tasks;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.DbTable;

namespace VE.BusinessLogicLayer.DB
{
    public class TestTableService
    {
        private readonly IRepository<TestTable> _testTableRepository;

        public TestTableService()
        {
            _testTableRepository = Factory.GetTestTableRepository();
        }

        public async Task<IEnumerable<TestTable>> GetAll()
        {
            return await _testTableRepository.GetAll();
        }

        public async Task<int> Insert(TestTable data)
        {
            return await _testTableRepository.Insert(data);
        }
    }
}