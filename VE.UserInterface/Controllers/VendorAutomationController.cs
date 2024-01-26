using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.Services;

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
            ViewBag.AppProspectiveVendor = appProspectiveVendor;
            return View();
        }
    }
}