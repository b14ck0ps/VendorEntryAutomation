using System.Threading.Tasks;
using System.Web.Mvc;
using VE.BusinessLogicLayer.DB;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.DbTable;

namespace VE.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var list = SharePointService.Instance.GetAllItemsFromList("Approver Info");
            var TestUser2 = SharePointService.Instance.AuthUserInformation("bergerbd\\azran");


            var TestUser = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation",TestUser2.Email);


            ViewBag.List = list;
            ViewBag.TestUser = TestUser.Username;
            ViewBag.TestDept = TestUser.DeptID;

            var TestTableData = new TestTable
            {
                Name = TestUser2.Title,
                PendingWith = TestUser2.UserName
            };

            var testTableService = new TestTableService();
    
            var result = await testTableService.Insert(TestTableData);

            ViewBag.TestTable = result > 0 ? "Success" : "Failed";


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