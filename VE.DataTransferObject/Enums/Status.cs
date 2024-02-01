namespace VE.DataTransferObject.Enums
{
    public enum Status
    {
        Created = 0,
        HODApproved = 1,
        VDTeamRFIFloat = 2,
        RFISubmittedByProspectiveVendor = 3,
        RFIProcessedByVD = 4,
        HPAndPApproved = 5,
        CSCOApproved = 6,
        VendorCreationInSAPAndRequestClosed = 7,
        ChangeRequestSentToRequestor = 8,
        ChangeRequestSentToProspectiveVendor = 9,
        ReSubmittedFromRequestor = 10,
        ReSubmittedFromVendor = 11,
        Rejected = 12
    }
}