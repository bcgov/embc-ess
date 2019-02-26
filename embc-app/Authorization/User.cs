namespace Gov.Jag.Embc.Public.Models
{
    /// <summary>
    /// User Extension (to support authorization)
    /// </summary>
    public sealed partial class User
    {
        /// <summary>
        /// User Permission Claim Property
        /// </summary>
        public const string PermissionClaim = "permission_claim";

        /// <summary>
        /// UserId Claim Property
        /// </summary>
        public const string UseridClaim = "userid_claim";

        /// <summary>
        /// User Type Claim Property
        /// </summary>
        public const string UserTypeClaim = "usertype_claim";
    }
}
