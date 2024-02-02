using System;
using System.Threading.Tasks;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
using VE.BusinessLogicLayer.Utilities;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;

namespace VE.BusinessLogicLayer.Handler
{
    public class ApprovarActionHandler
    {
        private readonly AppProspectiveVendorsService _appProspectiveVendorService;
        private readonly WorkflowHelper _workflowHelper;
        private readonly AppVendorEnlistmentLogsService _appVendorEnlistmentLogsService;
        private readonly string _baseUrl;
        private readonly string _loggedInUser;
        private readonly string _appProspectiveVendorCode;
        private readonly Status _currentStatus;
        private readonly string _comment;

        public ApprovarActionHandler(AppProspectiveVendorsService appProspectiveVendorService, WorkflowHelper workflowHelper, AppVendorEnlistmentLogsService appVendorEnlistmentLogsService, string baseUrl, string loggedInUser, string appProspectiveVendorCode, Status currentStatus, string comment)
        {
            _appProspectiveVendorService = appProspectiveVendorService;
            _workflowHelper = workflowHelper;
            _appVendorEnlistmentLogsService = appVendorEnlistmentLogsService;
            _baseUrl = baseUrl;
            _loggedInUser = loggedInUser;
            _appProspectiveVendorCode = appProspectiveVendorCode;
            _currentStatus = currentStatus;
            _comment = comment;
        }

        private async Task<bool> HandleAction(ApproverAction action)
        {
            try
            {
                _workflowHelper.AppProspectiveVendors = await _appProspectiveVendorService.GetByCode(_appProspectiveVendorCode);
                var nextApprover = _workflowHelper.GetNextPendingApprovalInfo(_currentStatus, action);
                await _appProspectiveVendorService.UpdateStatus(
                    nextApprover.Status,
                    _appProspectiveVendorCode,
                    int.Parse(nextApprover.PendingWithUserId ?? "0"));


                var employeeData = SharePointService.Instance.AuthUserInformation(_loggedInUser);

                var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                {
                    ProspectiveVendorId = 1,
                    Code = _appProspectiveVendorCode,
                    Status = (int)nextApprover.Status,
                    Comment = _comment,
                    Action = action.ToString(),
                    ActionById = employeeData.Email,
                    CreatorId = employeeData.Email,
                    CreationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    LastModificationTime = DateTime.Now
                };

                await _appVendorEnlistmentLogsService.Insert(appVendorEnlistmentLogsData);

                SharePointService.Instance.UpdatePendingApprovalByTitle(_appProspectiveVendorCode, nextApprover.Status.ToString(), nextApprover.PendingWithUserId);

                EmailHandler.SendEmail(nextApprover.PendingWithUserEmail, nextApprover.PendingWithUserDisplayName, _appProspectiveVendorCode, nextApprover.Status.ToString(), $"{_baseUrl}Home/Details/{_appProspectiveVendorCode}");

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> HandleApprove() => await HandleAction(ApproverAction.Approved);
        public async Task<bool> HandleChangeRequest() => await HandleAction(ApproverAction.ChangeRequest);
        public async Task<bool> HandleReject() => await HandleAction(ApproverAction.Rejected);
        public async Task<bool> HandleResubmitToVendor() => await HandleAction(ApproverAction.ReSubmit);
    }
}
