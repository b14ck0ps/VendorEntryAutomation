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
            var LoginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var Employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", LoginUser.Email);
            
            ViewBag.EmployeeData = Employee;


            //ViewBag.LoginUser = LoginUser;
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
                .FirstOrDefault(approver => approver["DeptId"] == AuthUserInfo.DeptId);


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


            var LoginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var Employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", LoginUser.Email);
            ViewBag.Employee = Employee.EmployeeName;
            ViewBag.TestDept = Employee.DeptId;

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