namespace Gov.Jag.Embc.Public.ViewModels
{
    public class FamilyMember : Person
    {
        public bool SameLastNameAsEvacuee { get; set; }
        public FamilyRelationshipType RelationshipToEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = Person.FAMILY_MEMBER;
        }
    }
}
