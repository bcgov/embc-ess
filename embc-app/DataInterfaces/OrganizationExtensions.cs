using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class OrganizationExtensions
    {
        public static void PatchValues(this Sqlite.Models.Organization self, ViewModels.Organization organization)
        {
            self.Active = organization.Active;
            self.Name = organization.Name;
            self.BceidAccountNumber = organization.BceidAccountNumber;
            self.Externaluseridentifier = organization.Externaluseridentifier;
        }

        public static void PatchValues(this ViewModels.Organization self, Sqlite.Models.Organization organization)
        {

            self.Active = organization.Active;
            self.Name = organization.Name;
            self.BceidAccountNumber = organization.BceidAccountNumber;
            self.Externaluseridentifier = organization.Externaluseridentifier;
        }

    }
}
