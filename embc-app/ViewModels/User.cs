namespace Gov.Jag.Embc.Public.ViewModels
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }
        public string businessname { get; set; }
        public bool isNewUser { get; set; }
        public bool isContactCreated { get; set; }
        public bool isAccountCreated { get; set; }
        public bool isBceidConfirmed { get; set; }
        public string contactid { get; set; }
        public string accountid { get; set; }
        public string UserType { get; set; }
        public string[] appRoles { get; set; }
        public int ClientTimeoutWarningInMinutes { get; set; }
        public int ClientTimeoutWarningDurationInMinutes { get; set; }
    }
}
