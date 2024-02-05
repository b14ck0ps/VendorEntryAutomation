using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;
using VE.DataTransferObject.SharePoint;

namespace VE.BusinessLogicLayer.Utilities
{
    public class WorkflowHelper
    {
        private static List<ListItem> ApproverInfo { get; set; }
        private static dynamic SC01Info { get; set; }
        public AppProspectiveVendors AppProspectiveVendors { get; set; }
        public IEnumerable<AppVendorEnlistmentLogs> AppVendorEnlistmentLog { get; set; }

        public WorkflowHelper()
        {
            ApproverInfo = SharePointService.Instance.GetAllItemsFromList("Approver Info");

            SC01Info = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(row => row["DeptID"] == "SC01" && row["Location"] == "Corporate");
        }

        public SpAuthUserInformation GetUserHod(string email)
        {
            var authUserInfo = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", email);
            var matchingDeptInfo = ApproverInfo
                .Cast<dynamic>()
                .FirstOrDefault(row => row["DeptID"] == authUserInfo.DeptId && row["Location"] == authUserInfo.BusAreaName);

            if (matchingDeptInfo == null) return null;
            var fieldUserValue = (FieldUserValue)matchingDeptInfo["HOD"];
            return new SpAuthUserInformation
            {
                Email = fieldUserValue.Email,
                UserId = fieldUserValue.LookupId,
                Title = fieldUserValue.LookupValue,
            };
        }

        private SpAuthUserInformation SC01_Hod()
        {
            var fieldUserValue = (FieldUserValue)SC01Info["HOD"];
            if (fieldUserValue == null) return null;
            return new SpAuthUserInformation
            {
                Email = fieldUserValue.Email,
                UserId = fieldUserValue.LookupId,
                Title = fieldUserValue.LookupValue
            };
        }

        private SpAuthUserInformation SC01_Approver1()
        {
            var fieldUserValue = (FieldUserValue)SC01Info["Approver1"];
            if (fieldUserValue == null) return null;
            return new SpAuthUserInformation
            {
                Email = fieldUserValue.Email,
                UserId = fieldUserValue.LookupId,
                Title = fieldUserValue.LookupValue
            };
        }

        private SpAuthUserInformation SC01_Approver6()
        {
            var fieldUserValue = (FieldUserValue)SC01Info["Approver6"];
            if (fieldUserValue == null) return null;
            return new SpAuthUserInformation
            {
                Email = fieldUserValue.Email,
                UserId = fieldUserValue.LookupId,
                Title = fieldUserValue.LookupValue
            };
        }

        public PendingApprovalInfo GetNextPendingApprovalInfo(Status currentStatus, ApproverAction action)
        {
            /* ! WORKFLOW CHAIN !
             * 1. If current status is Submitted, then next status is UserHOD Approved, and next approver is SC01_Approver1
             * 2. If current status is HodApproved, then next status is SendtoVendor, and next approver is SC01_Approver1
             * 3. If current status is VendorSubmitted, then next status is VDTeamApproved, and next approver is SC01_Approver6
             * 4. If current status is VDTeamApproved, then next status is HeadSPPApproved, and next approver is SC01_Hod
             * 5. If current status is HeadSPPApproved, then next status is DeptHeadApproved, and next approver is SC01_Approver1
             * 6. If current status is DeptHeadApproved, then next status is Completed, and next approver is null
             * 7. If current status is ReSubmitted, then next status is ChangeRequestSentToProspectiveVendor and next approver is null
             */
            var pendingApprovalInfo = new PendingApprovalInfo();

            switch (action)
            {
                case ApproverAction.Submitted:
                    break;
                case ApproverAction.Approved:
                    switch (currentStatus)
                    {
                        case Status.Created:
                            pendingApprovalInfo.Status = Status.HODApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver1().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver1().Email;
                            pendingApprovalInfo.PendingWithUserDisplayName = SC01_Approver1().Title;
                            break;
                        case Status.HODApproved:
                            pendingApprovalInfo.Status = Status.VDTeamRFIFloat;
                            pendingApprovalInfo.PendingWithUserId = "0";
                            pendingApprovalInfo.PendingWithUserEmail = "";
                            pendingApprovalInfo.PendingWithUserDisplayName = "Respective Vendor";
                            break;
                        case Status.RFISubmittedByProspectiveVendor:
                            pendingApprovalInfo.Status = Status.RFIProcessedByVD;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver6().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver6().Email;
                            pendingApprovalInfo.PendingWithUserDisplayName = SC01_Approver6().Title;
                            break;
                        case Status.RFIProcessedByVD:
                            pendingApprovalInfo.Status = Status.HPAndPApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Hod().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Hod().Email;
                            pendingApprovalInfo.PendingWithUserDisplayName = SC01_Hod().Title;
                            break;
                        case Status.HPAndPApproved:
                            pendingApprovalInfo.Status = Status.CSCOApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver1().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver1().Email;
                            pendingApprovalInfo.PendingWithUserDisplayName = SC01_Approver1().Title;
                            break;
                        case Status.CSCOApproved:
                            pendingApprovalInfo.Status = Status.VendorCreationInSAPAndRequestClosed;
                            pendingApprovalInfo.PendingWithUserId = null;
                            pendingApprovalInfo.PendingWithUserEmail = null;
                            break;
                        case Status.ReSubmittedFromRequestor:
                            var lastChangeRequestLog = AppVendorEnlistmentLog
                                .Where(log => log.Action == "ChangeRequest")
                                .OrderByDescending(log => log.CreationTime)
                                .FirstOrDefault();
                            if (lastChangeRequestLog == null) return null;

                            var previousLog = AppVendorEnlistmentLog
                                .Where(log => (log.Action == "Approved" || log.Action == "Submitted") && log.CreationTime < lastChangeRequestLog.CreationTime)
                                .OrderByDescending(log => log.CreationTime)
                                .FirstOrDefault();
                            return previousLog == null ? null : GetNextPendingApprovalInfo((Status)previousLog.Status, ApproverAction.Approved);
                        default:
                            throw new ArgumentOutOfRangeException(nameof(currentStatus), currentStatus, null);
                    }
                    break;
                case ApproverAction.ChangeRequest:
                    pendingApprovalInfo.Status = Status.ChangeRequestSentToRequestor;
                    pendingApprovalInfo.PendingWithUserId = AppProspectiveVendors.RequestorID.ToString();
                    break;
                case ApproverAction.Rejected:
                    pendingApprovalInfo.Status = Status.Rejected;
                    pendingApprovalInfo.PendingWithUserId = null;
                    pendingApprovalInfo.PendingWithUserEmail = null;
                    break;
                case ApproverAction.VendorReSubmit:
                    pendingApprovalInfo.Status = Status.ChangeRequestSentToProspectiveVendor;
                    pendingApprovalInfo.PendingWithUserId = null;
                    pendingApprovalInfo.PendingWithUserEmail = null;
                    break;
                case ApproverAction.RequesterReSubmit:
                    var lastLog = AppVendorEnlistmentLog.OrderByDescending(l => l.CreationTime).FirstOrDefault();
                    var lastApprover = SharePointService.Instance.GetUserByEmail("BergerEmployeeInformation", lastLog.CreatorId);
                    pendingApprovalInfo.Status = Status.ReSubmittedFromRequestor;
                    pendingApprovalInfo.PendingWithUserId = lastApprover.UserId;
                    pendingApprovalInfo.PendingWithUserEmail = lastApprover.Email;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            var delegateToUserId = GetDelegateToUserId(pendingApprovalInfo.PendingWithUserId);
            if (delegateToUserId == null) return pendingApprovalInfo;
            pendingApprovalInfo.PendingWithUserId = delegateToUserId.PendingWithUserId;
            pendingApprovalInfo.PendingWithUserEmail = delegateToUserId.PendingWithUserEmail;
            pendingApprovalInfo.PendingWithUserDisplayName = delegateToUserId.PendingWithUserDisplayName;
            return pendingApprovalInfo;
        }

        public PendingApprovalInfo GetDelegateToUserId(string userId)
        {
            var taskDelegationList = SharePointService.Instance.GetAllItemsFromList("Task Delegation");
            var today = DateTime.Now.Date;

            var delegationInfo = taskDelegationList
                .Select(row => new
                {
                    UserField = (FieldUserValue)row["User"],
                    DelegateTo = (FieldUserValue)row["Delegate_x0020_To"],
                    FromDate = Convert.ToDateTime(row["FormDate"]),
                    ToDate = Convert.ToDateTime(row["ToDate"])
                })
                .FirstOrDefault(info => Convert.ToString(info.UserField.LookupId) == userId && today >= info.FromDate && today <= info.ToDate);
            if (delegationInfo == null) return null;

            return new PendingApprovalInfo
            {
                PendingWithUserId = delegationInfo.DelegateTo.LookupId.ToString(),
                PendingWithUserEmail = delegationInfo.DelegateTo.Email,
                PendingWithUserDisplayName = delegationInfo.DelegateTo.LookupValue
            };
        }
    }
}
