using System;
using System.Net;
using Microsoft.SharePoint.Client;

namespace VE.DataAccessLayer
{
    internal class SpConnection
    {
        private const string Domain = "bergerbd";
        private const string SpUrl = "https://portaldv.bergerbd.com/leaveauto/";

        public static ClientContext GetContext()
        {
            var username = Environment.GetEnvironmentVariable("BERGERBD_USER");
            var password = Environment.GetEnvironmentVariable("BERGERBD_PASS");

            var ctx = new ClientContext(SpUrl);
            ctx.Credentials = new NetworkCredential(username, password, Domain);

            return ctx;
        }
    }
}
