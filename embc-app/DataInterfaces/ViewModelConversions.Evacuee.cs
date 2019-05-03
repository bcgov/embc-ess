using Gov.Jag.Embc.Public.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Gov.Jag.Embc.Public.Models.Db.Enumerations;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static partial class ViewModelConversions
    {
        public static ViewModels.Person ToViewModel(this Models.Db.Evacuee source, Models.Db.IncidentRegistration incidentRegistration)
        {
            ViewModels.Person result = null;
            var isHeadOfHousehold = false;
            if (source != null)
            {
                if (source.EvacueeTypeCode == FamilyRelationshipTypes.HeadOfHousehold.GetDisplayName())
                {
                    result = new ViewModels.HeadOfHousehold();
                    isHeadOfHousehold = true;
                }
                else
                {
                    result = new ViewModels.FamilyMember();
                }

                result.Id = source.IncidentRegistrationId.ToString();
                //TODO: Need to map EvacueeSequenceNumber to Person ViewModel
                result.FirstName = source.FirstName;
                result.LastName = source.LastName;

                if (isHeadOfHousehold)
                {
                    var resultHoh = result as ViewModels.HeadOfHousehold;
                    resultHoh.PhoneNumber = incidentRegistration.PhoneNumber;
                    resultHoh.PhoneNumberAlt = incidentRegistration.PhoneNumberAlt;
                    resultHoh.Email = incidentRegistration.Email;
                    // related entities
                    //resultHoh.PrimaryResidence = sourceHoh.PrimaryResidence.ToViewModel(); //TODO:  Get From Addresses
                    //resultHoh.MailingAddress = sourceHoh.MailingAddress?.ToViewModel(); //TODO:  Get From Addresses

                    //TODO:  Load family members from IncidentRegistration.Evacuees not HOH
                    var familyMembers = incidentRegistration.Evacuees.Where(e => e.EvacueeTypeCode != FamilyRelationshipTypes.HeadOfHousehold.GetDisplayName());
                    if (familyMembers.Any())
                    {
                        resultHoh.FamilyMembers = new List<ViewModels.FamilyMember>();
                        foreach (var familyMember in familyMembers)
                        {
                            resultHoh.FamilyMembers.Add(familyMember.ToViewModel(incidentRegistration) as ViewModels.FamilyMember);
                        }
                    }
                }

                var resultEvacuee = result as ViewModels.Evacuee;
                resultEvacuee.Nickname = source.Nickname;
                resultEvacuee.Initials = source.Initials;
                resultEvacuee.Gender = source.Gender;
                resultEvacuee.Dob = source.Dob
                    ;
                if (!isHeadOfHousehold)
                {
                    var resultFm = result as ViewModels.FamilyMember;
                    resultFm.RelationshipToEvacuee = source.EvacueeTypeCode == FamilyRelationshipTypes.ImmediateFamily.GetDisplayName()
                        ? FamilyRelationshipTypes.ImmediateFamily.ToViewModel() : FamilyRelationshipTypes.HeadOfHousehold.ToViewModel();
                    resultFm.SameLastNameAsEvacuee = source.SameLastNameAsEvacuee;
                }
            }
            return result;
        }
        public static Models.Db.Evacuee ToModel(this ViewModels.Person source)
        {
            Models.Db.Evacuee result = null;

            if (source != null)
            {
                result = new Models.Db.Evacuee();

                if (source.Id != null)
                {
                    result.IncidentRegistrationId = Guid.Parse(source.Id);
                }

                result.FirstName = source.FirstName;
                result.LastName = source.LastName;

                if (source is ViewModels.Evacuee sourceEvacuee)
                {
                    result.EvacueeSequenceNumber = sourceEvacuee.EvacueeSequenceNumber;
                    result.Nickname = sourceEvacuee.Nickname;
                    result.Initials = sourceEvacuee.Initials;
                    result.Gender = source.Gender;
                    result.Dob = sourceEvacuee.Dob;
                }

                if (source is ViewModels.HeadOfHousehold sourceHoh)
                {
                    result.EvacueeSequenceNumber = 1;
                    result.EvacueeTypeCode = FamilyRelationshipTypes.HeadOfHousehold.GetDisplayName();
                }

                if (source is ViewModels.FamilyMember sourceFm)
                {
                    result.EvacueeTypeCode = sourceFm.RelationshipToEvacuee.Code;
                    result.SameLastNameAsEvacuee = sourceFm.SameLastNameAsEvacuee;
                }
            }

            return result;
        }
    }
}
