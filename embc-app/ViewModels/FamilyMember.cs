namespace Gov.Jag.Embc.Public.ViewModels
{
    public class FamilyMember : Evacuee
    {
        public string RelationshipToEvacuee { get; set; }
        public bool SameLastNameAsEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = "FMBR";
        }
    }
}
