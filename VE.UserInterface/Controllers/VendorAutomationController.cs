using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.Services;
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

            var vendorCode = formCollection["AppProspectiveVendorCode"];
            var submitValue = formCollection["submitBtn"];

            if (!Enum.TryParse(submitValue, out ApproverAction action)) return RedirectToAction("Details", new { id = vendorCode });

            if (action == ApproverAction.Approved)
            {
                await appProspectiveVendorService.UpdateStatus(Status.HodApproved, vendorCode);
            }

            return RedirectToAction("Index");
        }

    }
}