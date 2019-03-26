namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public static class OrganizationExtensions
    {
        public static void PatchValues(this Models.Db.Organization self, ViewModels.Organization organization)
        {
            self.Active = organization.Active;
            self.Name = organization.Name;
            self.BceidAccountNumber = organization.BceidAccountNumber;
            self.Externaluseridentifier = organization.Externaluseridentifier;
        }

        public static void PatchValues(this ViewModels.Organization self, Models.Db.Organization organization)
        {
            self.Active = organization.Active;
            self.Name = organization.Name;
            self.BceidAccountNumber = organization.BceidAccountNumber;
            self.Externaluseridentifier = organization.Externaluseridentifier;
        }
    }
}
