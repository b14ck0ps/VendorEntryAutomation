using VE.DataAccessLayer.Interface;
using VE.DataAccessLayer.Repository;
using VE.DataTransferObject.DbTable;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer
{
    public static class Factory
    {
        public static ISharePointRepository GetSharePointRepository() => new SharePointRepository();
        public static ISharePointAuthenticationRepository GetSharePointAuthenticationRepository() => new SharePointAuthenticationRepository();
        public static IRepository<TestTable> GetTestTableRepository() => new TestTableRepository();

        public static IRepository<VendorEnlistment> GetVendorEnlistmentRepository() => new VendorEnlistmentRepository();
    }
}
