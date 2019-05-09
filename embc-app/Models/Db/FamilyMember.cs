namespace Gov.Jag.Embc.Public.Models.Db
{
    /// <summary>
    /// Person Database Model
    /// </summary>
    public class FamilyMember : EvacueeOld
    {
        public bool SameLastNameAsEvacuee { get; set; }

        // related entities
        public string RelationshipToEvacueeCode { get; set; }

        public FamilyRelationshipType RelationshipToEvacuee { get; set; }

        public FamilyMember()
        {
            PersonType = Person.FAMILY_MEMBER;
        }
    }
}
