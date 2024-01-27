﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint.Client;
using VE.BusinessLogicLayer.SharePoint;
using VE.DataTransferObject.Enums;
using VE.DataTransferObject.SharePoint;

namespace VE.BusinessLogicLayer.Utilities
{
    public class WorkflowHelper
    {
        private static List<ListItem> ApproverInfo { get; set; }
        private static dynamic SC01Info { get; set; }

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
                .FirstOrDefault(row => row["DeptID"] == authUserInfo.DeptId);

            if (matchingDeptInfo == null) return null;
            var fieldUserValue = (FieldUserValue)matchingDeptInfo["HOD"];
            return new SpAuthUserInformation
            {
                Email = fieldUserValue.Email,
                UserId = fieldUserValue.LookupId,
                Title = fieldUserValue.LookupValue
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
             * @ TODO:Need to add more workflow chain for other status & ChangeRequest
             */
            var pendingApprovalInfo = new PendingApprovalInfo();

            switch (action)
            {
                case ApproverAction.Submitted:
                    // TODO: use this for Resubmit after change request
                    break;
                case ApproverAction.Approved:
                    switch (currentStatus)
                    {
                        case Status.Submitted:
                            pendingApprovalInfo.Status = Status.HodApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver1().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver1().Email;
                            break;
                        case Status.HodApproved:
                            pendingApprovalInfo.Status = Status.SendtoVendor;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver1().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver1().Email;
                            break;
                        case Status.VendorSubmitted:
                            pendingApprovalInfo.Status = Status.VDTeamApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver6().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver6().Email;
                            break;
                        case Status.VDTeamApproved:
                            pendingApprovalInfo.Status = Status.HeadSPPApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Hod().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Hod().Email;
                            break;
                        case Status.HeadSPPApproved:
                            pendingApprovalInfo.Status = Status.DeptHeadApproved;
                            pendingApprovalInfo.PendingWithUserId = SC01_Approver1().UserId.ToString();
                            pendingApprovalInfo.PendingWithUserEmail = SC01_Approver1().Email;
                            break;
                        case Status.DeptHeadApproved:
                            pendingApprovalInfo.Status = Status.Completed;
                            pendingApprovalInfo.PendingWithUserId = null;
                            pendingApprovalInfo.PendingWithUserEmail = null;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(currentStatus), currentStatus, null);
                    }
                    break;
                case ApproverAction.ChangeRequest:
                    break;
                case ApproverAction.Rejected:
                    pendingApprovalInfo.Status = Status.Rejected;
                    pendingApprovalInfo.PendingWithUserId = null;
                    pendingApprovalInfo.PendingWithUserEmail = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            return pendingApprovalInfo;
        }
    }
}