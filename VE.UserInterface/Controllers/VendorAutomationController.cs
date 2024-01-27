using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;

namespace VE.UserInterface.Controllers
{
    public class VendorAutomationController : Controller
    {
        // GET: VendorAutomation
        public ActionResult Index()
        {
            return View();
        }
        // GET: VendorAutomation/id/{dynamic}
        public async Task<ActionResult> Details(string id)
        {
            var appProspectiveVendor = await new AppProspectiveVendorsService().GetByCode(id);
            var appProspectiveVendorMaterials = await new AppProspectiveVendorMaterialsService().GetByCode(id);
            var appVendorEnlistmentLogs = await new AppVendorEnlistmentLogsService().GetByCode(id);

            ViewBag.AppVendorEnlistmentLogs = appVendorEnlistmentLogs;
            ViewBag.AppProspectiveVendor = appProspectiveVendor;
            ViewBag.AppProspectiveVendorMaterials = appProspectiveVendorMaterials;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitAction(FormCollection formCollection)
        {
            var appProspectiveVendorService = new AppProspectiveVendorsService();

            var appProspectiveVendorCode = formCollection["AppProspectiveVendorCode"];
            var submitValue = formCollection["submitBtn"];
            var comment = formCollection["Comment"];

            if (!Enum.TryParse(submitValue, out ApproverAction action)) return RedirectToAction("Details", new { id = appProspectiveVendorCode });

            if (action == ApproverAction.Approved)
            {
                const Status status = Status.DeptHeadApproved; // TODO: Check the user role and set the status accordingly

                await appProspectiveVendorService.UpdateStatus(status, appProspectiveVendorCode);
                var employeeData = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
                var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                {
                    ProspectiveVendorId = 1,
                    Code = appProspectiveVendorCode,
                    Status = (int)status,
                    Comment = comment,
                    Action = ApproverAction.Submitted.ToString(),
                    ActionById = employeeData.Email,
                    CreatorId = employeeData.Email,
                    CreationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    LastModificationTime = DateTime.Now
                };
                await new AppVendorEnlistmentLogsService().Insert(appVendorEnlistmentLogsData);
            }

            return RedirectToAction("Index");
        }

    }
}