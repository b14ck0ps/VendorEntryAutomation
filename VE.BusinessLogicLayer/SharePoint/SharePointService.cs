using Microsoft.SharePoint.Client;
using System.Collections.Generic;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.SharePoint;

namespace VE.BusinessLogicLayer.SharePoint
{
    public class SharePointService
    {
        public static SharePointService Instance => new SharePointService();
        private readonly ISharePointAuthenticationRepository _sharePointAuthenticationRepository;

        public SharePointService()
        {
            _sharePointRepository = Factory.GetSharePointRepository();
            _sharePointAuthenticationRepository = Factory.GetSharePointAuthenticationRepository();
        }

        private readonly ISharePointRepository _sharePointRepository;

        public List<ListItem> GetAllItemsFromList(string listName)
        {
            return _sharePointRepository.GetAllItemsFromList(listName);
        }

        public UserInfo GetUserByEmail(string listName, string email)
        {
            var camlQuery = "<View><Query><Where><Eq><FieldRef Name='Email'/>" + "<Value Type='Text'>" + email +
                            "</Value></Eq></Where></Query></View>";

            var items = _sharePointRepository.GetItemsByQuery(listName, camlQuery);

            if (items.Count <= 0) return null;
            var userInfo = new UserInfo
            {
                Username = items[0]["EmployeeName"]?.ToString(),
                DeptID = items[0]["DeptID"]?.ToString(),
            };

            return userInfo;

        }

        public SpAuthUserInformation AuthUserInformation(string windowsUsername)
        {
            var user = _sharePointAuthenticationRepository.AuthUserInformation(windowsUsername);
            return new SpAuthUserInformation
            {
                Email = user.Email,
                Title = user.Title,
                UserId = user.Id,
                UserName = windowsUsername
            };
        }
    }
}
