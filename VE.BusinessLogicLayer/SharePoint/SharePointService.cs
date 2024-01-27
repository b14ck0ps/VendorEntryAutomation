using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using VE.DataAccessLayer;
using VE.DataAccessLayer.Interface;
using VE.DataTransferObject.SharePoint;

namespace VE.BusinessLogicLayer.SharePoint
{
    public class SharePointService
    {
        private readonly ISharePointAuthenticationRepository _sharePointAuthenticationRepository;

        private readonly ISharePointRepository _sharePointRepository;

        public SharePointService()
        {
            _sharePointRepository = Factory.GetSharePointRepository();
            _sharePointAuthenticationRepository = Factory.GetSharePointAuthenticationRepository();
        }

        public static SharePointService Instance => new SharePointService();

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
                EmployeeId = items[0]["EmployeeId"]?.ToString(),
                EmployeeName = items[0]["EmployeeName"]?.ToString(),
                Mobile = items[0]["Mobile"]?.ToString(),
                JobGrade = items[0]["EmployeeGrade"]?.ToString(),
                Designation = items[0]["Designation"]?.ToString(),
                DeptId = items[0]["DeptID"]?.ToString(),
                DeptName = items[0]["Department"]?.ToString(),
                Location = items[0]["OfficeLocation"]?.ToString(),
                Email = ((FieldUserValue)items[0]["Email"]).Email,
                OptManagerName = items[0]["OptManagerName"]?.ToString(),
                OptManagerEmail = ((FieldUserValue)items[0]["OptManagerEmail"]).Email,
                BusAreaName = items[0]["BusAreaName"]?.ToString()
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

        public void InsertItem(string listName, Dictionary<string, object> fieldValues)
        {
            _sharePointRepository.InsertItem(listName, fieldValues);
        }
    }
}