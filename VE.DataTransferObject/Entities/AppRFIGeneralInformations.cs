using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class AppRFIGeneralInformations
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ParentCompany { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public string ContactPersonTitle { get; set; }
        public int OrganizationTypeId { get; set; }
        public int ActivitiyCategoryId { get; set; }
        public bool IsAgent { get; set; }
        public string AgentStateName { get; set; }
        public string AgentAddressPrincipal { get; set; }
        public string AgentFileName { get; set; }
        public int FullTimeEmployeeNumber { get; set; }
        public int ProspectiveVendorId { get; set; }
    }
}
