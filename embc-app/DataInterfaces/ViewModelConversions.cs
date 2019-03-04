
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class ViewModelConversions
    {
        public static ViewModels.Community ToViewModel(this Sqlite.Models.Community item)
        {
            ViewModels.Community result = null;
            if (item != null)
            {
                result = new ViewModels.Community()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active,
                    Region = item.Region.ToViewModel()
                };
            }
            return result;
        }

        public static Sqlite.Models.Person ToModel(this ViewModels.Person item)
        {
            Sqlite.Models.Person result = null;
            if (item != null)
            {
                result = new Sqlite.Models.Person()
                {
                    Id = Guid.Parse(item.Id),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember
                };
            }
            return result;
        }

        public static ViewModels.Person ToViewModel(this Sqlite.Models.Person item)
        {
            ViewModels.Person result = null;
            if (item != null)
            {
                result = new ViewModels.Person()
                {
                    Id = item.Id.ToString(),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember
                };
            }
            return result;
        }

        public static Sqlite.Models.FamilyMember ToModel(this ViewModels.FamilyMember item)
        {
            Sqlite.Models.FamilyMember result = null;
            if (item != null)
            {
                result = new Sqlite.Models.FamilyMember()
                {
                    Id = Guid.Parse(item.Id),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    RelationshipToEvacuee = item.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = item.SameLastNameAsEvacuee,
                };
            }
            return result;
        }

        public static ViewModels.FamilyMember ToViewModel(this Sqlite.Models.FamilyMember item)
        {
            ViewModels.FamilyMember result = null;
            if (item != null)
            {
                result = new ViewModels.FamilyMember()
                {
                    Id = item.Id.ToString(),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Nickname = item.Nickname,
                    Initials = item.Initials,
                    Gender = item.Gender,
                    Dob = item.Dob,
                    IsEvacuee = item.IsEvacuee,
                    IsVolunteer = item.IsVolunteer,
                    IsFamilyMember = item.IsFamilyMember,
                    RelationshipToEvacuee = item.RelationshipToEvacuee,
                    SameLastNameAsEvacuee = item.SameLastNameAsEvacuee,
                };
            }
            return result;
        }

        public static ViewModels.Region ToViewModel(this Sqlite.Models.Region item)
        {
            ViewModels.Region result = null;
            if (item != null)
            {
                result = new ViewModels.Region()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active
                };
            }
            return result;
        }

        public static ViewModels.Registration ToViewModel(this Sqlite.Models.Registration item)
        {
            ViewModels.Registration result = null;
            if (item != null)
            {
                result = new ViewModels.Registration()
                {
                    Id = item.Id.ToString(),
                    IsRestricted = item.IsRestricted,
                    FamilyRepresentative = item.FamilyRepresentative.ToViewModel(),
                    IsRegisteringFamilyMembers = item.IsRegisteringFamilyMembers,
                    
                    InterviewerFirstName = item.InterviewerFirstName,
                    InterviewerLastNameInitial = item.InterviewerLastNameInitial,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,

        //specialNeeds: {};

                    IsSupportRequired = item.IsSupportRequired
                };
                
                if (item.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<ViewModels.FamilyMember>();
                    foreach (var familyMember in item.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToViewModel());
                    }
                }
            }


            return result;
        }

        public static Sqlite.Models.Registration ToModel(this ViewModels.Registration item)
        {
            Sqlite.Models.Registration result = null;
            if (item != null)
            {
                result = new Sqlite.Models.Registration()
                {
                    Id = Guid.Parse(item.Id),
                    IsRestricted = item.IsRestricted,
                    FamilyRepresentative = item.FamilyRepresentative.ToModel(),
                    IsRegisteringFamilyMembers = item.IsRegisteringFamilyMembers,

                    InterviewerFirstName = item.InterviewerFirstName,
                    InterviewerLastNameInitial = item.InterviewerLastNameInitial,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,

                    //specialNeeds: {};

                    IsSupportRequired = item.IsSupportRequired
                };

                if (item.FamilyMembers != null)
                {
                    result.FamilyMembers = new List<Sqlite.Models.FamilyMember>();
                    foreach (var familyMember in item.FamilyMembers)
                    {
                        result.FamilyMembers.Add(familyMember.ToModel());
                    }
                }
            }


            return result;
        }

        public static ViewModels.RegionalDistrict ToViewModel(this Sqlite.Models.RegionalDistrict item)
        {
            ViewModels.RegionalDistrict result = null;
            if (item != null)
            {
                result = new ViewModels.RegionalDistrict()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Active = item.Active
                };
            }
            return result;
        }

    }
}
