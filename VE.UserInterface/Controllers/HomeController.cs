using Microsoft.SharePoint.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.DB;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.DbTable;
using VE.DataTransferObject.Entities;

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
        public async Task<ActionResult> SubmitForm(string Name, string Email, string ProductMaterial, 
            string SupOtherReq, string Description, string SupGenralReq, string SupType, int ExistingSupCount, string ExistingSupProblem,string SupAddReason)
        {
            var AuthUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var AuthUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", AuthUser.Email);
            var ApproverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");

            var matchingDeptInfo = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(approver => approver["DeptID"] == AuthUserInfo.DeptId);


            var location = matchingDeptInfo["Location"];
            var department = matchingDeptInfo["Department"];
            var hod = ((FieldUserValue)matchingDeptInfo["HOD"]).Email;


            var vendorEnlistmentData = new VendorEnlistment
            {
                Name = Name,
                VendorCode = Name,
                Email = Email,
                ProductMaterial = ProductMaterial,
                Description = Description,
                SupGenralReq = SupGenralReq,
                SupOtherReq = SupOtherReq,
                SupType = SupType,
                ExistingSupCount = ExistingSupCount,
                ExistingSupProblem = ExistingSupProblem,
                SupAddReason = SupAddReason,
                Status = "Submitted",
                CreatedBy = Email,
                CreatedDate = DateTime.Now,
                UpdatedBy = Email,
                UpdatedDate = DateTime.Now,
                PendingWith = hod
            };

            var vendorEnlistmentService = new VendorEnlistmentService();
            var result = await vendorEnlistmentService.Insert(vendorEnlistmentData);

            /*var testTableData = new TestTable
            {
                Name = Name,
                PendingWith = hod
            };

            var testTableService = new TestTableService();
            var result = await testTableService.Insert(testTableData);*/



            if (result > 0)
            {
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }


          /*  var LoginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var Employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", LoginUser.Email);
            ViewBag.Employee = Employee.EmployeeName;
            ViewBag.TestDept = Employee.DeptId;*/

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