using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Volunteer ToViewModel(this Models.Db.Volunteer source)
        {
            var result = new ViewModels.Volunteer
            {
                Id = source.Id.ToString(),
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                BceidAccountNumber = source.BceidAccountUserName,
                BceidAccountUserName = source.BceidAccountUserName,
                Externaluseridentifier = source.UserId,
                SiteMinderGuid = source.UserId,
                IsAdministrator = source.IsAdministrator,
                IsPrimaryContact = source.IsPrimaryContact,
                CanAccessRestrictedFiles = source.CanAccessRestrictedFiles,
                Organization = mapper.Map<ViewModels.Organization>(source.Organization),
                Active = source.Active
            };

            return result;
        }

        public static Models.Db.Volunteer ToModel(this ViewModels.Volunteer source)
        {
            var result = new Models.Db.Volunteer
            {
                Id = string.IsNullOrEmpty(source.Id) ? 0 : int.Parse(source.Id),
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                BceidAccountUserName = source.BceidAccountNumber,
                UserId = source.Externaluseridentifier,
                IsAdministrator = source.IsAdministrator,
                IsPrimaryContact = source.IsPrimaryContact,
                CanAccessRestrictedFiles = source.CanAccessRestrictedFiles,
                OrganizationId = source.Organization == null ? (Guid?)null : Guid.Parse(source.Organization.Id),
                Active = source.Active.HasValue ? source.Active.Value : false
            };
            return result;
        }
    }
}
