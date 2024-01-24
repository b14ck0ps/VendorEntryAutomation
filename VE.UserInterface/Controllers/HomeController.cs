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
using VE.UserInterface.Enum;

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

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(VendorEnlistment formData, string comment)
        {
            var AuthUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var AuthUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", AuthUser.Email);
            var ApproverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");
            var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var matchingDeptInfo = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(approver => approver["DeptID"] == AuthUserInfo.DeptId);

            var location = matchingDeptInfo["Location"];
            var department = matchingDeptInfo["Department"];
            var hod = ((FieldUserValue)matchingDeptInfo["HOD"]).Email;

            // Generate a random vendor code
            string randomVendorCode = "VE-" + CodeGenerator.GenerateRandomCode();


            var vendorEnlistmentData = new VendorEnlistment
            {
                Name = formData.Name,
                VendorCode = randomVendorCode,
                Email = formData.Email,
                ProductMaterial = formData.ProductMaterial,
                Description = formData.Description,
                SupGenralReq = formData.SupGenralReq,
                SupOtherReq = formData.SupOtherReq,
                SupType = formData.SupType,
                ExistingSupCount = formData.ExistingSupCount,
                ExistingSupProblem = formData.ExistingSupProblem,
                SupAddReason = formData.SupAddReason,
                Status = "Submitted",
                CreatedBy = employeeData.Email,
                CreatedDate = DateTime.Now,
                UpdatedBy = employeeData.Email,
                UpdatedDate = DateTime.Now,
                PendingWith = hod
            };

            var vendorEnlistmentService = new VendorEnlistmentService();
            var result = await vendorEnlistmentService.Insert(vendorEnlistmentData);
            if (result > 0)
            {
                var vendorEnlistmentLogData = new VendorEnlistmentLog
                {
                    VendorCode = randomVendorCode,
                    Status = "Submitted",
                    Comment = comment,
                    ActionBy = formData.Email,
                    CreatedBy = employeeData.Email,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = employeeData.Email,
                    UpdatedDate = DateTime.Now
                };

                var vendorEnlistmentLogService = new VendorEnlistmentLogService();
                var resultLog = await vendorEnlistmentLogService.Insert(vendorEnlistmentLogData);
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }
            return View("Index");
        }*/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(AppProspectiveVendors formData, string comment)
        {
            var AuthUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var AuthUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", AuthUser.Email);
            var ApproverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");
            var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var matchingDeptInfo = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(approver => approver["DeptID"] == AuthUserInfo.DeptId);

            var location = matchingDeptInfo["Location"];
            var department = matchingDeptInfo["Department"];
            var hod = ((FieldUserValue)matchingDeptInfo["HOD"]).Email;

            // Generate a random vendor code
            string randomVendorCode = "VE-" + CodeGenerator.GenerateRandomCode();


            var appProspectiveVendorsData = new AppProspectiveVendors
            {
                ServiceDescription = formData.ServiceDescription,
                RequestorID = employeeData.UserId,
                Code = randomVendorCode,
                RequirementGeneral = formData.RequirementGeneral,
                RequirementOther = formData.RequirementOther,
                TypeOfSupplierId = formData.TypeOfSupplierId,
                ExisitngSupplierCount = formData.ExisitngSupplierCount,
                ExisitngSupplierProblem = formData.ExisitngSupplierProblem,
                NewSupplierAdditionReason = formData.NewSupplierAdditionReason,
                VendorName = formData.VendorName,
                VendorEmail = formData.VendorEmail,
                Status = (int)StatusEnum.Submitted,
                ExtraProperties = "",
                ConcurrencyStamp = "",
                CreatorId = employeeData.Email,
                CreationTime = DateTime.Now,
                LastModifierId = employeeData.Email,
                LastModificationTime = DateTime.Now,
                IsDeleted = false,
                PendingWithUserId = hod,
                IsIncludedIntoSAP = false
            };

            var appProspectiveVendorsService = new AppProspectiveVendorsService();
            var result = await appProspectiveVendorsService.Insert(appProspectiveVendorsData);

            if (result > 0)
            {
                var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                {
                    ProspectiveVendorId = 1,
                    Code = randomVendorCode,
                    Status = (int)StatusEnum.Submitted,
                    Comment = comment,
                    Action = "Submitted",
                    ActionById = employeeData.Email,
                    CreatorId = employeeData.Email,
                    ExtraProperties = "",
                    ConcurrencyStamp = "",
                    CreationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    LastModificationTime = DateTime.Now
                };

                var appVendorEnlistmentLogsService = new AppVendorEnlistmentLogsService();
                var resultLog = await appVendorEnlistmentLogsService.Insert(appVendorEnlistmentLogsData);
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }
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