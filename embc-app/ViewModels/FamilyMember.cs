namespace Gov.Jag.Embc.Public.ViewModels
{
    public class FamilyMember : Evacuee
    {
        public bool SameLastNameAsEvacuee { get; set; }
        public FamilyRelationshipType RelationshipToEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = Evacuee.FAMILY_MEMBER;
        }
    }
}
