using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Person ToViewModel(this Models.Db.Person source)
        {
            ViewModels.Person result = null;
            if (source != null)
            {
                result = ViewModels.Person.Create(source.PersonType);

                result.Id = source.Id.ToString();
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Nickname = source.Nickname;
                result.Initials = source.Initials;
                result.Gender = source.Gender;
                result.Dob = source.Dob;
                result.Active = source.Active;

                // TODO: Add fields for HOH, FMBR, VOLN
                if (source is Models.Db.HeadOfHousehold sourceHoh)
                {
                    var resultHoh = result as ViewModels.HeadOfHousehold;
                    resultHoh.PhoneNumber = sourceHoh.PhoneNumber;
                    resultHoh.PhoneNumberAlt = sourceHoh.PhoneNumberAlt;
                    resultHoh.Email = sourceHoh.Email;
                    // related entities
                    resultHoh.PrimaryResidence = sourceHoh.PrimaryResidence.ToViewModel();
                    resultHoh.MailingAddress = sourceHoh.MailingAddress?.ToViewModel();
                    if (sourceHoh.FamilyMembers != null)
                    {
                        resultHoh.FamilyMembers = new List<ViewModels.FamilyMember>();
                        foreach (var familyMember in sourceHoh.FamilyMembers)
                        {
                            resultHoh.FamilyMembers.Add(familyMember.ToViewModel() as ViewModels.FamilyMember);
                        }
                    }
                }
                if (source is Models.Db.FamilyMember sourceFm)
                {
                    var resultFm = result as ViewModels.FamilyMember;
                    resultFm.RelationshipToEvacuee = sourceFm.RelationshipToEvacuee.ToViewModel();
                    resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
                if (source is Models.Db.Volunteer sourceVol)
                {
                    var resultVol = result as ViewModels.Volunteer;
                    resultVol.Name = sourceVol.Name;
                    resultVol.Email = sourceVol.Email;
                    resultVol.BceidAccountNumber = sourceVol.BceidAccountNumber;
                    resultVol.Externaluseridentifier = sourceVol.Externaluseridentifier;
                    resultVol.IsAdministrator = sourceVol.IsAdministrator;
                    resultVol.IsPrimaryContact = sourceVol.IsPrimaryContact;
                    resultVol.CanAccessRestrictedFiles = sourceVol.CanAccessRestrictedFiles;
                    // related entities
                    resultVol.Organization = sourceVol.Organization.ToViewModel();
                }
            }
            return result;
        }

        public static Models.Db.Person ToModel(this ViewModels.Person source)
        {
            Models.Db.Person result = null;
            if (source != null)
            {
                result = Models.Db.Person.Create(source.PersonType);

                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Nickname = source.Nickname;
                result.Initials = source.Initials;
                result.Gender = source.Gender;
                result.Dob = source.Dob;
                result.Active = source.Active;

                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }

                // TODO: Add fields for HOH, FMBR, VOLN
                if (source is ViewModels.HeadOfHousehold sourceHoh)
                {
                    var resultHoh = result as Models.Db.HeadOfHousehold;
                    resultHoh.PhoneNumber = sourceHoh.PhoneNumber;
                    resultHoh.PhoneNumberAlt = sourceHoh.PhoneNumberAlt;
                    resultHoh.Email = sourceHoh.Email;
                    // related entities
                    resultHoh.PrimaryResidence = sourceHoh.PrimaryResidence.ToModel();
                    resultHoh.MailingAddress = sourceHoh.MailingAddress?.ToModel();
                    if (sourceHoh.FamilyMembers != null)
                    {
                        resultHoh.FamilyMembers = new List<Models.Db.FamilyMember>();
                        foreach (var familyMember in sourceHoh.FamilyMembers)
                        {
                            resultHoh.FamilyMembers.Add(familyMember.ToModel() as Models.Db.FamilyMember);
                        }
                    }
                }
                if (source is ViewModels.FamilyMember sourceFm)
                {
                    var resultFm = result as Models.Db.FamilyMember;
                    resultFm.RelationshipToEvacueeCode = sourceFm.RelationshipToEvacuee.Code;
                    resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
                if (source is ViewModels.Volunteer sourceVol)
                {
                    var resultVol = result as Models.Db.Volunteer;
                    resultVol.Name = sourceVol.Name;
                    resultVol.Email = sourceVol.Email;
                    resultVol.BceidAccountNumber = sourceVol.BceidAccountNumber;
                    resultVol.Externaluseridentifier = sourceVol.Externaluseridentifier;
                    resultVol.IsAdministrator = sourceVol.IsAdministrator;
                    resultVol.IsPrimaryContact = sourceVol.IsPrimaryContact;
                    resultVol.CanAccessRestrictedFiles = sourceVol.CanAccessRestrictedFiles;
                    // related entities
                    resultVol.OrganizationId = sourceVol.Organization == null ? (Guid?)null : Guid.Parse(sourceVol.Organization.Id);
                }
            }
            return result;
        }
    }
}
