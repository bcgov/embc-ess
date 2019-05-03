using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

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
                if (source is Models.Db.EvacueeOld evacuee)
                {
                    var resultEvacuee = result as ViewModels.Evacuee;
                    resultEvacuee.Nickname = evacuee.Nickname;
                    resultEvacuee.Initials = evacuee.Initials;
                    resultEvacuee.Gender = evacuee.Gender;
                    resultEvacuee.Dob = evacuee.Dob;
                }
                if (source is Models.Db.FamilyMember sourceFm)
                {
                    var resultFm = result as ViewModels.FamilyMember;
                    resultFm.RelationshipToEvacuee = sourceFm.RelationshipToEvacuee.ToViewModel();
                    resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
            }
            return result;
        }


        //public static Models.Db.Person ToModelOld(this ViewModels.Person source)
        //{
        //    Models.Db.Person result = null;
        //    if (source != null)
        //    {
        //        result = Models.Db.Person.Create(source.PersonType);

        //        if (source.Id != null)
        //        {
        //            result.Id = Guid.Parse(source.Id);
        //        }
        //        result.FirstName = source.FirstName;
        //        result.LastName = source.LastName;

        //        if (source is ViewModels.Evacuee sourceEvacuee)
        //        {
        //            var resultEvacuee = result as Models.Db.Evacuee;
        //            resultEvacuee.Nickname = sourceEvacuee.Nickname;
        //            resultEvacuee.Initials = sourceEvacuee.Initials;
        //            resultEvacuee.Gender = source.Gender;
        //            resultEvacuee.Dob = sourceEvacuee.Dob;
        //        }
        //        if (source is ViewModels.HeadOfHousehold sourceHoh)
        //        {
        //            var resultHoh = result as Models.Db.HeadOfHousehold;
        //            resultHoh.PhoneNumber = sourceHoh.PhoneNumber;
        //            resultHoh.PhoneNumberAlt = sourceHoh.PhoneNumberAlt;
        //            resultHoh.Email = sourceHoh.Email;
        //            // related entities
        //            resultHoh.PrimaryResidence = sourceHoh.PrimaryResidence.ToModel();
        //            resultHoh.MailingAddress = sourceHoh.MailingAddress?.ToModel();
        //            if (sourceHoh.FamilyMembers != null)
        //            {
        //                resultHoh.FamilyMembers = new List<Models.Db.FamilyMember>();
        //                foreach (var familyMember in sourceHoh.FamilyMembers)
        //                {
        //                    resultHoh.FamilyMembers.Add(familyMember.ToModel() as Models.Db.FamilyMember);
        //                }
        //            }
        //        }
        //        if (source is ViewModels.FamilyMember sourceFm)
        //        {
        //            var resultFm = result as Models.Db.FamilyMember;
        //            resultFm.RelationshipToEvacueeCode = sourceFm.RelationshipToEvacuee.Code;
        //            resultFm.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
        //        }
        //    }
        //    return result;
        //}

        public static Models.Db.Evacuee ToModel(this ViewModels.Person source)
        {
            Models.Db.Evacuee result = null;

            if(source != null)
            {
                result = new Models.Db.Evacuee();

                //TODO: Evacuee RegistrationId
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;

                if (source is ViewModels.Evacuee sourceEvacuee)
                {
                    result.Nickname = sourceEvacuee.Nickname;
                    result.Initials = sourceEvacuee.Initials;
                    result.Gender = source.Gender;
                    result.Dob = sourceEvacuee.Dob;
                }

                if (source is ViewModels.HeadOfHousehold sourceHoh)
                {
                    result.EvacueeSequenceNumber = 1;
                    result.EvacueeTypeCode = FamilyRelationshipTypes.HeadOfHousehold.GetDisplayName();
                    //var resultHoh = result as Models.Db.HeadOfHousehold;
                    //resultHoh.PhoneNumber = sourceHoh.PhoneNumber;  //TODO - Goes to IncidentRegistration
                    //resultHoh.PhoneNumberAlt = sourceHoh.PhoneNumberAlt; //TODO - Goes to IncidentRegistration
                    //resultHoh.Email = sourceHoh.Email; //TODO - Goes to IncidentRegistration
                    // related entities
                    //result.PrimaryResidence = sourceHoh.PrimaryResidence.ToModel();  //TODO - Goes to RegistrationAddresses.Type
                    //result.MailingAddress = sourceHoh.MailingAddress?.ToModel();  //TODO - Goes to RegistrationAddresses.Type
                    //if (sourceHoh.FamilyMembers != null)
                    //{
                    //    resultHoh.FamilyMembers = new List<Models.Db.FamilyMember>();
                    //    foreach (var familyMember in sourceHoh.FamilyMembers)
                    //    {
                    //        resultHoh.FamilyMembers.Add(familyMember.ToModel() as Models.Db.FamilyMember);
                    //    }
                    //}
                }

                if (source is ViewModels.FamilyMember sourceFm)
                {
                    //var resultFm = result as Models.Db.FamilyMember;
                    //resultFm.RelationshipToEvacueeCode = sourceFm.RelationshipToEvacuee.Code;
                    result.EvacueeTypeCode = sourceFm.RelationshipToEvacuee.Code;
                    result.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
            }

            return result;
        }
    }
}
