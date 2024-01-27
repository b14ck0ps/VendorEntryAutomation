using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.Handler;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
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

            var actionEnabled = false;

            switch (appProspectiveVendor.Status)
            {
                case (int)Status.Rejected:
                    break;
                case (int)Status.Completed:
                    break;
                case (int)Status.SendtoVendor:
                    ViewBag.PendingWithVendor = true;
                    break;
                default:
                    //actionEnabled = appProspectiveVendor.PendingWithUserId == loginUser.Email; //TODO: Use this after testing
                    actionEnabled = true;
                    break;
            }


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
            var code = await FormSubmissionHandler.HandleFormSubmission(User.Identity.Name, formData, comment, SelectedMaterials);
            return string.IsNullOrEmpty(code) ? RedirectToAction("Index") : RedirectToAction("Details", new { id = code });
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