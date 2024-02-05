using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VE.BusinessLogicLayer.Services;
using VE.BusinessLogicLayer.SharePoint;
using VE.BusinessLogicLayer.Utilities;
using VE.DataTransferObject.Entities;
using VE.DataTransferObject.Enums;
using VE.DataTransferObject.SharePoint;

namespace VE.BusinessLogicLayer.Handler
{
    public class FormSubmissionHandler
    {
        public static async Task<string> HandleFormSubmission(string loggedInUser, AppProspectiveVendors formData, string comment,
            string[] selectedMaterials, string baseUrl)
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(loggedInUser);
            var employeeData = SharePointService.Instance.AuthUserInformation(loggedInUser);

            var workflowHelper = new WorkflowHelper();
            var hod = workflowHelper.GetUserHod(loginUser.Email);
            var delegateToUser = workflowHelper.GetDelegateToUserId(hod.UserId.ToString());

            if (delegateToUser != null)
            {
                var delegatetedUser = new SpAuthUserInformation
                {
                    UserId = int.Parse(delegateToUser.PendingWithUserId),
                    Email = delegateToUser.PendingWithUserEmail,
                    Title = delegateToUser.PendingWithUserDisplayName,
                };
                hod = delegatetedUser;
            }

            var randomVendorCode = "VE-" + CodeGenerator.GenerateRandomCode();

            var appProspectiveVendorsData = new AppProspectiveVendors
            {
                ServiceDescription = formData.ServiceDescription,
                RequestorID = employeeData.UserId,
                Code = randomVendorCode,
                RequirementGeneral = formData.RequirementGeneral,
                RequirementOther = formData.RequirementOther,
                TypeOfSupplierId = formData.TypeOfSupplierId,
                ExisitngSupplierCount = formData.ExisitngSupplierCount,
                ExisitngSupplierProblem = formData.ExisitngSupplierProblem,
                NewSupplierAdditionReason = formData.NewSupplierAdditionReason,
                VendorName = formData.VendorName,
                VendorEmail = formData.VendorEmail,
                Status = (int)Status.Created,
                ExtraProperties = "",
                ConcurrencyStamp = "",
                CreatorId = employeeData.Email,
                CreationTime = DateTime.Now,
                LastModifierId = employeeData.Email,
                LastModificationTime = DateTime.Now,
                IsDeleted = false,
                PendingWithUserId = hod.UserId.ToString(),
                IsIncludedIntoSAP = false
            };

            var appProspectiveVendorsService = new AppProspectiveVendorsService();
            var result = await appProspectiveVendorsService.Insert(appProspectiveVendorsData);

            if (result <= 0) return null;
            var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
            {
                ProspectiveVendorId = 1,
                Code = randomVendorCode,
                Status = (int)Status.Created,
                Comment = comment,
                Action = "Submitted",
                ActionById = employeeData.Email,
                CreatorId = employeeData.Email,
                ExtraProperties = "",
                ConcurrencyStamp = "",
                CreationTime = DateTime.Now,
                LastModifierId = employeeData.Email,
                LastModificationTime = DateTime.Now
            };
            foreach (var material in selectedMaterials)
            {
                var appProspectiveVendorMaterial = new AppProspectiveVendorMaterials
                {
                    ProspectiveVendorId = 1,
                    MaterialCode = material.Split('|')[0],
                    MaterialName = material.Split('|')[1],
                    CreationTime = DateTime.Now,
                    CreatorId = employeeData.Email,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    VendorCode = randomVendorCode
                };
                var appProspectiveVendorMaterialsService = new AppProspectiveVendorMaterialsService();
                await appProspectiveVendorMaterialsService.Insert(appProspectiveVendorMaterial);
            }


            var appVendorEnlistmentLogsService = new AppVendorEnlistmentLogsService();
            await appVendorEnlistmentLogsService.Insert(appVendorEnlistmentLogsData);

            var pendingApprovalList = new Dictionary<string, object>
            {
                { "Title", randomVendorCode },
                { "ProcessName", "Vendor Enlistment" },
                { "RequestedByName", employeeData.Title },
                { "Status", Status.Created.ToString()},
                { "EmployeeID", employeeData.UserId.ToString() },
                { "RequestedByEmail", employeeData.Email },
                { "PendingWith", hod.UserId.ToString() },
                { "RequestLink", $"{baseUrl}Home/Details/{randomVendorCode}" }
            };

            SharePointService.Instance.InsertItem("PendingApproval", pendingApprovalList);

            EmailHandler.SendEmail(hod.Email, hod.Title, randomVendorCode, Status.Created.ToString(), $"{baseUrl}Home/Details/{randomVendorCode}");

            return randomVendorCode;

        }

        public static async Task<string> HandleFormSubmissionForChangeRequest(string loggedInUser,
            AppProspectiveVendors formData, string comment,
            string[] selectedMaterials, string baseUrl)
        {
            var employeeData = SharePointService.Instance.AuthUserInformation(loggedInUser);
            var workflowHelper = new WorkflowHelper();
            workflowHelper.AppVendorEnlistmentLog = await new AppVendorEnlistmentLogsService().GetByCode(formData.Code);

            var netApprovalInfo = workflowHelper.GetNextPendingApprovalInfo((Status)formData.Status, ApproverAction.RequesterReSubmit);
            var appProspectiveVendorsData = new AppProspectiveVendors
            {
                ServiceDescription = formData.ServiceDescription,
                RequestorID = employeeData.UserId,
                Code = formData.Code,
                RequirementGeneral = formData.RequirementGeneral,
                RequirementOther = formData.RequirementOther,
                TypeOfSupplierId = formData.TypeOfSupplierId,
                ExisitngSupplierCount = formData.ExisitngSupplierCount,
                ExisitngSupplierProblem = formData.ExisitngSupplierProblem,
                NewSupplierAdditionReason = formData.NewSupplierAdditionReason,
                VendorName = formData.VendorName,
                VendorEmail = formData.VendorEmail,
                Status = (int)Status.ReSubmittedFromRequestor,
                ExtraProperties = "",
                ConcurrencyStamp = "",
                CreatorId = employeeData.Email,
                CreationTime = DateTime.Now,
                LastModifierId = employeeData.Email,
                LastModificationTime = DateTime.Now,
                IsDeleted = false,
                PendingWithUserId = netApprovalInfo.PendingWithUserId,
                IsIncludedIntoSAP = false
            };

            var appProspectiveVendorsService = new AppProspectiveVendorsService();
            var result = await appProspectiveVendorsService.Update(appProspectiveVendorsData);

            if (result <= 0) return null;
            var appVendorEnlistmentLogsData = new AppVendorEnlistmentLogs
            {
                ProspectiveVendorId = 1,
                Code = formData.Code,
                Status = (int)Status.ReSubmittedFromRequestor,
                Comment = comment,
                Action = "Updated",
                ActionById = employeeData.Email,
                CreatorId = employeeData.Email,
                ExtraProperties = "",
                ConcurrencyStamp = "",
                CreationTime = DateTime.Now,
                LastModifierId = employeeData.Email,
                LastModificationTime = DateTime.Now
            };
            await new AppVendorEnlistmentLogsService().Insert(appVendorEnlistmentLogsData);
            await new AppProspectiveVendorMaterialsService().Delete(formData.Code);

            foreach (var material in selectedMaterials)
            {
                var appProspectiveVendorMaterial = new AppProspectiveVendorMaterials
                {
                    ProspectiveVendorId = 1,
                    MaterialCode = material.Split('|')[0],
                    MaterialName = material.Split('|')[1],
                    CreationTime = DateTime.Now,
                    CreatorId = employeeData.Email,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = employeeData.Email,
                    VendorCode = formData.Code
                };
                await new AppProspectiveVendorMaterialsService().Insert(appProspectiveVendorMaterial);
            }


            SharePointService.Instance.UpdatePendingApprovalByTitle(formData.Code, Status.ReSubmittedFromRequestor.ToString(), netApprovalInfo.PendingWithUserId);

            EmailHandler.SendEmail(netApprovalInfo.PendingWithUserEmail, netApprovalInfo.PendingWithUserDisplayName, formData.Code, Status.ReSubmittedFromRequestor.ToString(), $"{baseUrl}Home/Details/{formData.Code}");

            return formData.Code;
        }
    }
}