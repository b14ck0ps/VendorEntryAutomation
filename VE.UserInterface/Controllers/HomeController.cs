using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using VE.BusinessLogicLayer.Handler;
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
            var employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", loginUser.Email);

            //var actionEnabled = (appProspectiveVendor.PendingWithUserId == loginUser.Email) or (appProspectiveVendor.Status != (int)Status.Completed);//TODO: Remove comment after Testing
            var actionEnabled = appProspectiveVendor.Status != (int)Status.Completed;

            ViewBag.AppVendorEnlistmentLogs = appVendorEnlistmentLogs;
            ViewBag.AppProspectiveVendor = appProspectiveVendor;
            ViewBag.AppProspectiveVendorMaterials = appProspectiveVendorMaterials;
            ViewBag.LoginUser = loginUser;
            ViewBag.EmployeeData = employee;
            ViewBag.ActionEnabled = actionEnabled;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitForm(AppProspectiveVendors formData, string comment,
            List<string> SelectedMaterials)
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            ViewBag.LoginUser = loginUser;

            var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var workflowHelper = new WorkflowHelper();
            var hod = workflowHelper.GetUserHod(loginUser.Email);
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
                PendingWithUserId = hod.UserId.ToString(),
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
                    await appProspectiveVendorMaterialsService.Insert(appProspectiveVendorMaterial);
                }


                var appVendorEnlistmentLogsService = new AppVendorEnlistmentLogsService();
                var resultLog = await appVendorEnlistmentLogsService.Insert(appVendorEnlistmentLogsData);
                ViewBag.SubmitResult = "Form submitted successfully";
            }
            else
            {
                ViewBag.SubmitResult = "Failed to submit form";
            }

            return RedirectToAction("Details", new { id = randomVendorCode });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitAction(FormCollection formCollection)
        {
            var appProspectiveVendorCode = formCollection["AppProspectiveVendorCode"];
            var submitValue = formCollection["submitBtn"];
            var currentStatus = formCollection["CurrentStatus"];
            var comment = formCollection["Comment"];

            if (!Enum.TryParse(submitValue, out ApproverAction action))
                return RedirectToAction("Index", new { id = appProspectiveVendorCode });


            switch (action)
            {
                case ApproverAction.Submitted:
                    break;
                case ApproverAction.Approved:
                    await ApprovarActionHandler.HandleApprove(User.Identity.Name, appProspectiveVendorCode, (Status)Enum.Parse(typeof(Status), currentStatus), comment);
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