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
        public static async Task<bool> HandleApprove(string loggedInUser, string appProspectiveVendorCode, Status currentStatus, string comment)
        {
            try
            {
                var appProspectiveVendorService = new AppProspectiveVendorsService();

                var workflowHelper = new WorkflowHelper();
                var nextApprover = workflowHelper.GetNextPendingApprovalInfo(currentStatus, ApproverAction.Approved);


                await appProspectiveVendorService.UpdateStatus(nextApprover.Status, appProspectiveVendorCode);
                var employeeData = SharePointService.Instance.AuthUserInformation(loggedInUser);
                var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                {
                    ProspectiveVendorId = 1,
                    Code = appProspectiveVendorCode,
                    Status = (int)nextApprover.Status,
                    Comment = comment,
                    Action = ApproverAction.Approved.ToString(),
                    ActionById = employeeData.Email,
                    CreatorId = employeeData.Email,
                    CreationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    LastModificationTime = DateTime.Now
                };
                await new AppVendorEnlistmentLogsService().Insert(appVendorEnlistmentLogsData);

                SharePointService.Instance.UpdatePendingApprovalByTitle(appProspectiveVendorCode, nextApprover.Status.ToString(), nextApprover.PendingWithUserId);

                EmailHandler.SendEmail(nextApprover.PendingWithUserEmail, nextApprover.PendingWithUserDisplayName, appProspectiveVendorCode, nextApprover.Status.ToString(), "https://localhost:44300/Home/Details/" + appProspectiveVendorCode);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> HandleReject(string loggedInUser, string appProspectiveVendorCode, Status currentStatus, string comment)
        {
            try
            {
                var appProspectiveVendorService = new AppProspectiveVendorsService();

                var workflowHelper = new WorkflowHelper();
                var nextApprover = workflowHelper.GetNextPendingApprovalInfo(currentStatus, ApproverAction.Rejected);


                await appProspectiveVendorService.UpdateStatus(nextApprover.Status, appProspectiveVendorCode);
                var employeeData = SharePointService.Instance.AuthUserInformation(loggedInUser);
                var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
                {
                    ProspectiveVendorId = 1,
                    Code = appProspectiveVendorCode,
                    Status = (int)nextApprover.Status,
                    Comment = comment,
                    Action = ApproverAction.Rejected.ToString(),
                    ActionById = employeeData.Email,
                    CreatorId = employeeData.Email,
                    CreationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    LastModificationTime = DateTime.Now
                };
                await new AppVendorEnlistmentLogsService().Insert(appVendorEnlistmentLogsData);

                SharePointService.Instance.UpdatePendingApprovalByTitle(appProspectiveVendorCode, nextApprover.Status.ToString(), nextApprover.PendingWithUserId);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
