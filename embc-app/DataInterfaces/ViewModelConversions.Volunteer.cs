using System;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        //TODO: merge into PersonConversions
        [Obsolete]
        public static ViewModels.Volunteer ToViewModel(this Sqlite.Models.Volunteer source)
        {
            ViewModels.Volunteer result = null;
            if (source != null)
            {
                result = new ViewModels.Volunteer()
                {
                    Id = source.Id.ToString(),
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Nickname = source.Nickname,
                    Initials = source.Initials,
                    Gender = source.Gender,
                    Dob = source.Dob,
                    Name = source.Name,
                    Email = source.Email,
                    BceidAccountNumber = source.BceidAccountNumber,
                    IsAdministrator = source.IsAdministrator,
                    IsPrimaryContact = source.IsPrimaryContact,
                    CanAccessRestrictedFiles = source.CanAccessRestrictedFiles,
                    Externaluseridentifier = source.Externaluseridentifier
                };

                // related entities
                if (source.Organization != null)
                {
                    result.Organization = source.Organization.ToViewModel();
                }
            }
            return result;
        }
    }
}
