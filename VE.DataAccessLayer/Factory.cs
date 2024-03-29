﻿using VE.DataAccessLayer.Interface;
using VE.DataAccessLayer.Repository;
using VE.DataTransferObject.Entities;

namespace VE.DataAccessLayer
{
    public static class Factory
    {
        public static ISharePointRepository GetSharePointRepository()
        {
            return new SharePointRepository();
        }

        public static ISharePointAuthenticationRepository GetSharePointAuthenticationRepository()
        {
            return new SharePointAuthenticationRepository();
        }

        public static IAppProspectiveVendorRepository GetAppProspectiveVendorsRepository()
        {
            return new AppProspectiveVendorsRepository();
        }

        public static IAppRFIGeneralInformationRepository GetAppRFIGeneralInformationRepository()
        {
            return new AppRFIGeneralInformationRepository();
        }

        public static IAppRFICertificatesRepository GetAppRFICertificatesRepository()
        {
            return new AppRFICertificatesRepository();
        }

        public static IAppRFIExperienceProductAvailabilitiesRepository GetAppRFIExperienceProductAvailabilitiesRepository()
        {
            return new AppRFIExperienceProductAvailabilitiesRepository();
        }
        public static IAppRFILegalEstablishmentRepository GetAppRFILegalEstablishmentRepository()
        {
            return new AppRFILegalEstablishmentRepository();
        }
        public static IAppVendorEnlistmentLogsRepository GetAppVendorEnlistmentLogsRepository()
        {
            return new AppVendorEnlistmentLogsRepository();
        }

        public static IAppProspectiveVendorMaterialsRepository GetAppProspectiveVendorMaterialsRepository()
        {
            return new AppProspectiveVendorMaterialsRepository();
        }

        public static IRepository<VendorEnlistment> GetVendorEnlistmentRepository()
        {
            return new VendorEnlistmentRepository();
        }

        public static IRepository<VendorEnlistmentLog> GetVendorEnlistmentLogRepository()
        {
            return new VendorEnlistmentLogRepository();
        }
    }
}