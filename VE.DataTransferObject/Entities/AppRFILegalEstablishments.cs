using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE.DataTransferObject.Entities
{
    public class AppRFILegalEstablishments
    {
        public int Id { get; set; }
        public string TradeLicenseNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ValidityPeriod { get; set; }
        public string TradeLicenseFileName { get; set; }
        public string IncorporationCertificate { get; set; }
        public string IncomeTaxCertificate { get; set; }
        public string IRCFileName { get; set; }
        public string ERCFileName { get; set; }
        public string BankSolvencyCertificate { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BankAddress { get; set; }
        public string AnnualTurnover { get; set; }
        public bool IsAuditedFinancialStatement { get; set; }
        public string AuditedFinancialStatementFileName { get; set; }
        public string PendingLawsuitFileName { get; set; }
        public int ProspectiveVendorId { get; set; }
    }
}
