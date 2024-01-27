using System;
using System.Collections.Generic;
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

                var pendingApprovalList = new Dictionary<string, object>
                {
                    { "Title", appProspectiveVendorCode },
                    { "ProcessName", "Vendor Enlistment" },
                    { "RequestedByName", employeeData.Title },
                    { "Status", Enum.GetName(typeof(Status) ,nextApprover.Status)},
                    { "EmployeeID", employeeData.UserId.ToString() },
                    { "RequestedByEmail", employeeData.Email },
                    { "PendingWith", nextApprover.PendingWithUserId },
                    { "RequestLink", "http://localhost:44317/Home/Details/" + appProspectiveVendorCode }
                };

                SharePointService.Instance.InsertItem("PendingApproval", pendingApprovalList);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
