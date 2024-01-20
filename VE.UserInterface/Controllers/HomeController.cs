using Microsoft.SharePoint.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.DB;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.DbTable;

namespace VE.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var TestUser2 = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var TestUser = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", TestUser2.Email);
            ViewBag.TestUser = TestUser.Username;
            ViewBag.TestDept = TestUser.DeptID;


            ViewBag.TestUser2 = TestUser2;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(string Name)
        {
            var AuthUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var AuthUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", AuthUser.Email);
            var ApproverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");

            var matchingDeptInfo = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(approver => approver["DeptID"] == AuthUserInfo.DeptID);


            var location = matchingDeptInfo["Location"];
            var department = matchingDeptInfo["Department"];
            var hod = ((FieldUserValue)matchingDeptInfo["HOD"]).Email;

            var testTableData = new TestTable
            {
                Name = Name,
                PendingWith = hod
            };

            var testTableService = new TestTableService();
            var result = await testTableService.Insert(testTableData);

            if (result > 0)
            {
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }


            var TestUser2 = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var TestUser = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", TestUser2.Email);
            ViewBag.TestUser = TestUser.Username;
            ViewBag.TestDept = TestUser.DeptID;

            return View("Index");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}