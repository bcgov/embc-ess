using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Volunteer ToViewModel(this Models.Db.Volunteer source)
        {
            ViewModels.Volunteer result = null;
            if (source != null)
            {
                result.Id = source.Id.ToString();
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Email = source.Email;
                result.BceidAccountNumber = source.BceidAccountUserName;
                result.BceidAccountUserName = source.BceidAccountUserName;
                result.Externaluseridentifier = source.UserId?.ToString();
                result.SiteMinderGuid = source.UserId?.ToString();
                result.IsAdministrator = source.IsAdministrator;
                result.IsPrimaryContact = source.IsPrimaryContact;
                result.CanAccessRestrictedFiles = source.CanAccessRestrictedFiles;
                result.Organization = source.Organization.ToViewModel();
                result.Active = source.Active;
            }
            return result;
        }

        public static Models.Db.Volunteer ToModel(this ViewModels.Volunteer source)
        {
            Models.Db.Volunteer result = null;
            if (source != null)
            {
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Email = source.Email;
                result.BceidAccountUserName = source.BceidAccountNumber;
                result.UserId = source.Externaluseridentifier == null ? (Guid?)null : Guid.Parse(source.Externaluseridentifier);
                result.IsAdministrator = source.IsAdministrator;
                result.IsPrimaryContact = source.IsPrimaryContact;
                result.CanAccessRestrictedFiles = source.CanAccessRestrictedFiles;
                result.OrganizationId = source.Organization == null ? (Guid?)null : Guid.Parse(source.Organization.Id);
                if (source.Active.HasValue)
                {
                    result.Active = source.Active.Value;
                }
            }
            return result;
        }
    }
}
