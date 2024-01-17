using System.Web.Mvc;
using VE.BusinessLogicLayer.SharePoint;

namespace VE.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var list = SharePointService.Instance.GetAllItemsFromList("Approver Info");
            var TestUser2 = SharePointService.Instance.AuthUserInformation("bergerbd\\azran");


            var TestUser = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation",TestUser2.Email);


            ViewBag.List = list;
            ViewBag.TestUser = TestUser.Username;
            ViewBag.TestDept = TestUser.DeptID;

            ViewBag.TestUser2 = TestUser2;
            return View();
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