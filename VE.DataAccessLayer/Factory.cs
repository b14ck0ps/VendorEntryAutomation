using VE.DataAccessLayer.Interface;
using VE.DataAccessLayer.Repository;

namespace VE.DataAccessLayer
{
    public static class Factory
    {
        public static ISharePointRepository GetSharePointRepository() => new SharePointRepository();
        public static ISharePointAuthenticationRepository GetSharePointAuthenticationRepository() => new SharePointAuthenticationRepository();
    }
}
