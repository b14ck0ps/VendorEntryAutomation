using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class AppRFICertificates
    {
        public int Id { get; set; }
        public string StandardApproveFileName { get; set; }
        public bool IsCompanyDocumentedQAPolicy { get; set; }
        public bool IsCompanyMembershipWithNationalOrInternational { get; set; }
        public bool IsCompanyCoveredInsurance { get; set; }
        public string CompanyCoveredInsuranceFileName { get; set; }
        public bool IsCompanyDocumentedEnvironmentalPolicy { get; set; }
        public string CompanyDocumentedEnvironmentalPolicyFileName { get; set; }
        public bool IsCompanyRelatedMember { get; set; }
        public string CompanyRelatedMemberDetails { get; set; }
        public int ProspectiveVendorId { get; set; }
    }
}
