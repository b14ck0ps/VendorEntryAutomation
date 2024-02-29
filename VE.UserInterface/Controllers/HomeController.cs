using Newtonsoft.Json;
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
        public async Task<ActionResult> Index(string code)
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            ViewBag.LoginUser = loginUser;
            var employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", loginUser.Email);
            var materialMaster = SharePointService.Instance.GetAllItemsFromList("MaterialMasterTest");
            ViewBag.MaterialMaster = materialMaster;
            ViewBag.EmployeeData = employee;

            if (string.IsNullOrEmpty(code)) return View();
            var appProspectiveVendors = await new AppProspectiveVendorsService().GetByCode(code);
            var appProspectiveVendorMaterials = await new AppProspectiveVendorMaterialsService().GetByCode(code);
            var appVendorEnlistmentLogs = await new AppVendorEnlistmentLogsService().GetByCode(code);
            if (appProspectiveVendors == null) return View();
            if (appVendorEnlistmentLogs != null) ViewBag.appVendorEnlistmentLogs = appVendorEnlistmentLogs;
            if (appProspectiveVendors.Status != (int)Status.ChangeRequestSentToRequestor/* || appProspectiveVendors.PendingWithUserId != loginUser.UserId.ToString()*/) return View();//TODO: use this authorization
            ViewBag.AppProspectiveVendors = appProspectiveVendors;
            ViewBag.AppProspectiveVendorMaterials = appProspectiveVendorMaterials;

            return View();
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var appProspectiveVendorMaterials = await new AppProspectiveVendorMaterialsService().GetByCode(id);
            var appVendorEnlistmentLogs = await new AppVendorEnlistmentLogsService().GetByCode(id);
            var loginUser = SharePointService.Instance.AuthUserInformation(User.Identity.Name);
            var employee = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", loginUser.Email);
            var appProspectiveVendor = await new AppProspectiveVendorsService().GetByCode(id);

            var appProspectiveVendorId = appProspectiveVendor.ProspectiveVendorId;

            var appRFIGeneralInformationList = new List<AppRFIGeneralInformations>();
            appRFIGeneralInformationList.Add(await new AppRFIGeneralInformationService().GetById(appProspectiveVendorId));

            var appRFILegalEstablishmentsList = new List<AppRFILegalEstablishments>();
            appRFILegalEstablishmentsList.Add(await new AppRFILegalEstablishmentService().GetById(appProspectiveVendorId));

            var appRFIExperienceProductAvailabilitiesList = new List<AppRFIExperienceProductAvailabilities>();
            appRFIExperienceProductAvailabilitiesList.Add(await new AppRFIExperienceProductAvailabilitiesService().GetById(appProspectiveVendorId));

            var appRFICertificatesList = new List<AppRFICertificates>();
            appRFICertificatesList.Add(await new AppRFICertificatesService().GetById(appProspectiveVendorId));


            if (appProspectiveVendor.Status == (int)Status.ChangeRequestSentToRequestor /*&& appProspectiveVendor.PendingWithUserId == loginUser.UserId.ToString()*/)//TODO: use this authorization
                return RedirectToAction("Index", new { appProspectiveVendor.Code });



            var actionEnabled = false;

            switch (appProspectiveVendor.Status)
            {
                case (int)Status.Rejected:
                    break;
                case (int)Status.VendorCreationInSAPAndRequestClosed:
                    break;
                case (int)Status.VDTeamRFIFloat:
                    ViewBag.PendingWithVendor = true;
                    break;
                case (int)Status.ChangeRequestSentToProspectiveVendor:
                    ViewBag.PendingWithVendor = true;
                    break;
                default:
                    actionEnabled = true;
                    //actionEnabled = (appProspectiveVendor.PendingWithUserId == loginUser.UserId.ToString()&&appProspectiveVendor.Status != (int)Status.ChangeRequestSentToRequestor); //TODO: use this authorization
                    break;
            }

            var pendingWithUser = SharePointService.Instance.GetByUserId("BergerEmployeeInformation", appProspectiveVendor.PendingWithUserId);
            ViewBag.PendingWith = pendingWithUser?.EmployeeName ?? "N/A";
            ViewBag.AppVendorEnlistmentLogs = appVendorEnlistmentLogs;

            if (appRFIGeneralInformationList[0] != null) ViewBag.AppRFIGeneralInformation = appRFIGeneralInformationList;
            if (appRFILegalEstablishmentsList[0] != null) ViewBag.AppRFILegalEstablishments = appRFILegalEstablishmentsList;
            if (appRFIExperienceProductAvailabilitiesList[0] != null) ViewBag.AppRFIExperienceProductAvailabilities = appRFIExperienceProductAvailabilitiesList;
            if (appRFICertificatesList[0] != null) ViewBag.AppRFICertificates = appRFICertificatesList;

            ViewBag.AppProspectiveVendor = appProspectiveVendor;
            ViewBag.AppProspectiveVendorMaterials = appProspectiveVendorMaterials;
            ViewBag.LoginUser = loginUser;
            ViewBag.EmployeeData = employee;
            ViewBag.ActionEnabled = actionEnabled;


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SubmitForm(AppProspectiveVendors formData, string comment,
            string SelectedMaterials)
        {
            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
            var baseUrl = urlBuilder.ToString();
            var selectedMaterialsArray = JsonConvert.DeserializeObject<string[]>(SelectedMaterials);

            var code = await FormSubmissionHandler.HandleFormSubmission(User.Identity.Name, formData, comment, selectedMaterialsArray, baseUrl);
            return Json(new { code = code });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitFormForChangeRequest(AppProspectiveVendors formData, string comment,
            string SelectedMaterials)
        {
            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
            var baseUrl = urlBuilder.ToString();
            var selectedMaterialsArray = JsonConvert.DeserializeObject<string[]>(SelectedMaterials);

            var code = await FormSubmissionHandler.HandleFormSubmissionForChangeRequest(User.Identity.Name, formData, comment, selectedMaterialsArray, baseUrl);
            return Json(new { code = code });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitAction(FormCollection formCollection)
        {
            var appProspectiveVendorCode = formCollection["AppProspectiveVendorCode"];
            var submitValue = formCollection["submitBtn"];
            var currentStatus = formCollection["CurrentStatus"];
            var comment = formCollection["Comment"];

            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri) { Path = Request.ApplicationPath, Query = null, Fragment = null };
            var baseUrl = urlBuilder.ToString();

            if (!Enum.TryParse(submitValue, out ApproverAction action))
                return RedirectToAction("Index", new { id = appProspectiveVendorCode });

            var approvarActionHandler = new ApprovarActionHandler(baseUrl, User.Identity.Name, appProspectiveVendorCode, (Status)Enum.Parse(typeof(Status), currentStatus), comment);
            switch (action)
            {
                case ApproverAction.Submitted:
                    break;
                case ApproverAction.Approved:
                    await approvarActionHandler.HandleApprove();
                    break;
                case ApproverAction.ChangeRequest:
                    await approvarActionHandler.HandleChangeRequest();
                    break;
                case ApproverAction.Rejected:
                    await approvarActionHandler.HandleReject();
                    break;
                case ApproverAction.VendorReSubmit:
                    await approvarActionHandler.HandleResubmitToVendor();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return RedirectToAction("Details", new { id = appProspectiveVendorCode });
        }
    }
}