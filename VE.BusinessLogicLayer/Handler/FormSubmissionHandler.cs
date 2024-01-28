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
    public class FormSubmissionHandler
    {
        public static async Task<string> HandleFormSubmission(string loggedInUser, AppProspectiveVendors formData, string comment,
            string[] selectedMaterials, string baseUrl)
        {
            var loginUser = SharePointService.Instance.AuthUserInformation(loggedInUser);

            var employeeData = SharePointService.Instance.AuthUserInformation(loggedInUser);
            var workflowHelper = new WorkflowHelper();
            var hod = workflowHelper.GetUserHod(loginUser.Email);
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
                Status = (int)Status.Submitted,
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
                Status = (int)Status.Submitted,
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
                { "Status", Status.Submitted.ToString()},
                { "EmployeeID", employeeData.UserId.ToString() },
                { "RequestedByEmail", employeeData.Email },
                { "PendingWith", hod.UserId.ToString() },
                { "RequestLink", $"{baseUrl}Home/Details/{randomVendorCode}" }
            };

            SharePointService.Instance.InsertItem("PendingApproval", pendingApprovalList);

            EmailHandler.SendEmail(hod.Email, hod.Title, randomVendorCode, Status.Submitted.ToString(), $"{baseUrl}Home/Details/{randomVendorCode}");

            return randomVendorCode;
        }
    }
}