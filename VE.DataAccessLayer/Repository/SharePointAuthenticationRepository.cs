﻿using Microsoft.SharePoint.Client;
using VE.DataAccessLayer.Interface;

namespace VE.DataAccessLayer.Repository
{
    internal class SharePointAuthenticationRepository : ISharePointAuthenticationRepository
    {
        public User AuthUserInformation(string windowsUsername)
        {
            //windowsUsername = "BERGERBD\\azran"; //TODO: use windowsUsername, not hard-coded value
            using (var ctx = SpConnection.GetContext())
            {
                var spUser = ctx.Web.EnsureUser(windowsUsername);
                ctx.Load(spUser, u => u.Email, u => u.Title, u => u.Id);
                ctx.ExecuteQuery();
                return spUser;
            }
        }
    }
}