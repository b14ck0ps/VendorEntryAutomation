using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
using VE.BusinessLogicLayer.Utilities;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;
using FormCollection = System.Web.Mvc.FormCollection;

namespace VE.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            ViewBag.LoginUser = loginUser;
            var employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", loginUser.Email);
            var materialMaster = SharePointService.Instance.GetAllItemsFromList("MaterialMaster");

            ViewBag.MaterialMaster = materialMaster;
            ViewBag.EmployeeData = employee;

            return View();
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var appProspectiveVendor = await new AppProspectiveVendorsService().GetByCode(id);
            var appProspectiveVendorMaterials = await new AppProspectiveVendorMaterialsService().GetByCode(id);
            var appVendorEnlistmentLogs = await new AppVendorEnlistmentLogsService().GetByCode(id);
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);

            ViewBag.AppVendorEnlistmentLogs = appVendorEnlistmentLogs;
            ViewBag.AppProspectiveVendor = appProspectiveVendor;
            ViewBag.AppProspectiveVendorMaterials = appProspectiveVendorMaterials;
            ViewBag.LoginUser = loginUser;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(AppProspectiveVendors formData, string comment,
            List<string> SelectedMaterials)
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            ViewBag.LoginUser = loginUser;
            var authUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", loginUser.Email);
            var approverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");
            var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var matchingDeptInfo = approverInfo
                .Cast<dynamic>()
                .FirstOrDefault(approver => approver["DeptID"] == authUserInfo.DeptId);

            var location = matchingDeptInfo["Location"];
            var department = matchingDeptInfo["Department"];
            var hod = ((FieldUserValue)matchingDeptInfo["HOD"]).Email;

            // Generate a random vendor code
            var randomVendorCode = "VE-" + CodeGenerator.GenerateRandomCode();


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
                Status = (int)Status.Submitted,
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
                    Status = (int)Status.Submitted,
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
                var resultMaterial = 0;
                foreach (var material in SelectedMaterials)
                {
                    var appProspectiveVendorMaterial = new AppProspectiveVendorMaterials
                    {
                        ProspectiveVendorId = 1,
                        MaterialCode = material.Split('|')[0],
                        MaterialName = material.Split('|')[1],
                        CreationTime = DateTime.Now,
                        CreatorId = employeeData.Email,
                        LastModificationTime = DateTime.Now,
                        LastModifierId = employeeData.Email,
                        VendorCode = randomVendorCode
                    };
                    var appProspectiveVendorMaterialsService = new AppProspectiveVendorMaterialsService();
                    resultMaterial = await appProspectiveVendorMaterialsService.Insert(appProspectiveVendorMaterial);
                }


                var appVendorEnlistmentLogsService = new AppVendorEnlistmentLogsService();
                var resultLog = await appVendorEnlistmentLogsService.Insert(appVendorEnlistmentLogsData);
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }

            return RedirectToAction("Details",new { id = randomVendorCode });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitAction(FormCollection formCollection)
        {
            var appProspectiveVendorService = new AppProspectiveVendorsService();

            var appProspectiveVendorCode = formCollection["AppProspectiveVendorCode"];
            var submitValue = formCollection["submitBtn"];
            var comment = formCollection["Comment"];

            if (!Enum.TryParse(submitValue, out ApproverAction action))
                return RedirectToAction("Index", new { id = appProspectiveVendorCode });

            switch (action)
            {
                case ApproverAction.Submitted:
                    break;
                case ApproverAction.Approved:
                    const Status status = Status.VDTeamApproved; // TODO: Check the user role and set the status accordingly
                    await appProspectiveVendorService.UpdateStatus(status, appProspectiveVendorCode);
                    var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
                    var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                    {
                        ProspectiveVendorId = 1,
                        Code = appProspectiveVendorCode,
                        Status = (int)status,
                        Comment = comment,
                        Action = action.ToString(),
                        ActionById = employeeData.Email,
                        CreatorId = employeeData.Email,
                        CreationTime = DateTime.Now,
                        LastModifierId = employeeData.Email,
                        LastModificationTime = DateTime.Now
                    };
                    await new AppVendorEnlistmentLogsService().Insert(appVendorEnlistmentLogsData);

                    var pendingApprovalList = new Dictionary<string, object>
                    {
                        {"Title", appProspectiveVendorCode},
                        {"ProcessName", "Vendor Enlistment"},
                        {"RequestedByName", employeeData.Title},
                        {"Status", "Pending"},
                        {"EmployeeID", employeeData.UserId.ToString()},
                        {"RequestedByEmail", employeeData.Email},
                        {"PendingWith", employeeData.UserId},
                        {"RequestLink", "http://localhost:44317/Home/Details/" + appProspectiveVendorCode}
                    };

                    SharePointService.Instance.InsertItem("PendingApproval", pendingApprovalList);

                    break;
                case ApproverAction.ChangeRequest:
                    break;
                case ApproverAction.Rejected:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return RedirectToAction("Details", new { id = appProspectiveVendorCode });
        }
    }
}