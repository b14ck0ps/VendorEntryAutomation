using VE.DataTransferObject.Enums;

namespace VE.DataTransferObject.SharePoint
{
    public class PendingApprovalInfo
    {
        public Status Status { get; set; }
        public string PendingWithUserId { get; set; }
        public string PendingWithUserEmail { get; set; }
        public string PendingWithUserDisplayName { get; set; }
    }
}
