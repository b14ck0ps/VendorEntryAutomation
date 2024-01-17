using Microsoft.SharePoint.Client;

namespace VE.DataAccessLayer.Interface
{
    public interface ISharePointAuthenticationRepository
    {
        User AuthUserInformation(string windowsUsername);
    }
}
