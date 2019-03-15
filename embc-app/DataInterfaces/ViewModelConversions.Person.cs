using System;
using System.Collections.Generic;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Person ToViewModel(this Sqlite.Models.Person source)
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

                // TODO: Add fields for HOH, FMBR, VOLN
                if (source is Sqlite.Models.HeadOfHousehold sourceHoh)
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
                if (source is Sqlite.Models.FamilyMember sourceFm)
                {
                    var resultFm = result as ViewModels.FamilyMember;
                    resultFm.RelationshipToEvacuee = sourceFm.RelationshipToEvacuee.ToViewModel();
                    resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;

                }
                if (source is Sqlite.Models.Volunteer sourceVol)
                {
                    var resultVol = result as ViewModels.Volunteer;
                    resultVol.Name = sourceVol.Name;
                    resultVol.Email = sourceVol.Email;
                    resultVol.BceidAccountNumber = sourceVol.BceidAccountNumber;
                    resultVol.IsAdministrator = sourceVol.IsAdministrator;
                    resultVol.IsPrimaryContact = sourceVol.IsPrimaryContact;
                    resultVol.CanAccessRestrictedFiles = sourceVol.CanAccessRestrictedFiles;
                    // related entities
                    resultVol.Organization = sourceVol.Organization.ToViewModel();

                }
            }
            return result;
        }

        public static Sqlite.Models.Person ToModel(this ViewModels.Person source)
        {
            Sqlite.Models.Person result = null;
            if (source != null)
            {
                result = Sqlite.Models.Person.Create(source.PersonType);

                result.FirstName = source.FirstName;
                result.LastName = source.LastName;
                result.Nickname = source.Nickname;
                result.Initials = source.Initials;
                result.Gender = source.Gender;
                result.Dob = source.Dob;
                if (source.Id != null)
                {
                    result.Id = Guid.Parse(source.Id);
                }

                // TODO: Add fields for HOH, FMBR, VOLN
                if (source is ViewModels.HeadOfHousehold sourceHoh)
                {
                    var resultHoh = result as Sqlite.Models.HeadOfHousehold;
                    resultHoh.PhoneNumber = sourceHoh.PhoneNumber;
                    resultHoh.PhoneNumberAlt = sourceHoh.PhoneNumberAlt;
                    resultHoh.Email = sourceHoh.Email;
                    // related entities
                    resultHoh.PrimaryResidence = sourceHoh.PrimaryResidence.ToModel();
                    // resultHoh.MailingAddress = sourceHoh.MailingAddress?.ToModel();
                    if (sourceHoh.FamilyMembers != null)
                    {
                        resultHoh.FamilyMembers = new List<Sqlite.Models.FamilyMember>();
                        foreach (var familyMember in sourceHoh.FamilyMembers)
                        {
                            resultHoh.FamilyMembers.Add(familyMember.ToModel() as Sqlite.Models.FamilyMember);
                        }
                    }

                }
                if (source is ViewModels.FamilyMember sourceFm)
                {
                    var resultFm = result as Sqlite.Models.FamilyMember;
                    resultFm.RelationshipToEvacuee = sourceFm.RelationshipToEvacuee.ToModel();
                    resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
                if (source is ViewModels.Volunteer sourceVol)
                {
                    var resultVol = result as Sqlite.Models.Volunteer;
                    resultVol.Name = sourceVol.Name;
                    resultVol.Email = sourceVol.Email;
                    resultVol.BceidAccountNumber = sourceVol.BceidAccountNumber;
                    resultVol.IsAdministrator = sourceVol.IsAdministrator;
                    resultVol.IsPrimaryContact = sourceVol.IsPrimaryContact;
                    resultVol.CanAccessRestrictedFiles = sourceVol.CanAccessRestrictedFiles;
                    // related entities
                    resultVol.Organization = sourceVol.Organization.ToModel();
                }
            }
            return result;
        }

        // public static Sqlite.Models.FamilyMember ToModel(this ViewModels.FamilyMember source)
        // {
        //     Sqlite.Models.FamilyMember result = null;
        //     if (source != null)
        //     {
        //         result = new Sqlite.Models.FamilyMember()
        //         {
        //             FirstName = source.FirstName,
        //             LastName = source.LastName,
        //             Nickname = source.Nickname,
        //             Initials = source.Initials,
        //             Gender = source.Gender,
        //             Dob = source.Dob,
        //             RelationshipToEvacuee = source.RelationshipToEvacuee,
        //             SameLastNameAsEvacuee = source.SameLastNameAsEvacuee,
        //         };
        //         if (source.Id != null)
        //         {
        //             result.Id = Guid.Parse(source.Id);
        //         }
        //     }
        //     return result;
        // }

        // public static ViewModels.FamilyMember ToViewModel(this Sqlite.Models.FamilyMember source)
        // {
        //     ViewModels.FamilyMember result = null;
        //     if (source != null)
        //     {
        //         result = new ViewModels.FamilyMember()
        //         {
        //             Id = source.Id.ToString(),
        //             FirstName = source.FirstName,
        //             LastName = source.LastName,
        //             Nickname = source.Nickname,
        //             Initials = source.Initials,
        //             Gender = source.Gender,
        //             Dob = source.Dob,
        //             RelationshipToEvacuee = source.RelationshipToEvacuee,
        //             SameLastNameAsEvacuee = source.SameLastNameAsEvacuee,
        //         };
        //     }
        //     return result;
        // }

        // public static ViewModels.HeadOfHousehold ToViewModel(this Sqlite.Models.HeadOfHousehold source)
        // {
        //     ViewModels.HeadOfHousehold result = null;
        //     if (source != null)
        //     {
        //         result = new ViewModels.HeadOfHousehold()
        //         {
        //             Id = source.Id.ToString(),
        //             FirstName = source.FirstName,
        //             LastName = source.LastName,
        //             Nickname = source.Nickname,
        //             Initials = source.Initials,
        //             Gender = source.Gender,
        //             Dob = source.Dob,
        //             PhoneNumber = source.PhoneNumber,
        //             PhoneNumberAlt = source.PhoneNumberAlt,
        //             Email = source.Email,

        //             PrimaryResidence = source.PrimaryResidence.ToViewModel(),
        //             MailingAddress = source.MailingAddress.ToViewModel()
        //         };
        //     }
        //     return result;
        // }

        // public static Sqlite.Models.HeadOfHousehold ToModel(this ViewModels.HeadOfHousehold source)
        // {
        //     Sqlite.Models.HeadOfHousehold result = null;
        //     if (source != null)
        //     {
        //         result = new Sqlite.Models.HeadOfHousehold()
        //         {
        //             FirstName = source.FirstName,
        //             LastName = source.LastName,
        //             Nickname = source.Nickname,
        //             Initials = source.Initials,
        //             Gender = source.Gender,
        //             Dob = source.Dob,
        //             PhoneNumber = source.PhoneNumber,
        //             PhoneNumberAlt = source.PhoneNumberAlt,
        //             Email = source.Email,

        //             PrimaryResidence = source.PrimaryResidence.ToModel(),
        //             MailingAddress = source.MailingAddress.ToModel()
        //         };
        //         if (source.Id != null)
        //         {
        //             result.Id = Guid.Parse(source.Id);
        //         }
        //     }
        //     return result;
        // }
    }
}
