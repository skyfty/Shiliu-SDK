namespace Shiliu.Oral.Sdk.Abstractions.Models
{
    public class UserInfoResult
    {
        public UserInfo UserInfo { get; set; }
        public RightsAndInterests RightsAndInterests { get; set; }
        public bool ExistBindCode { get; set; }
        public bool ExistProfile { get; set; }
        public object HasDigitalHumanPermission { get; set; }
        public string Token { get; set; }
        public int Type { get; set; }
        public object Duration { get; set; }
        public object HaveUseDuration { get; set; }
        public string ExpireTime { get; set; }
        public InviteCodeResponse InviteCodeResponse { get; set; }
    }

    public class UserInfo
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public object Img { get; set; }
        public string Email { get; set; }
        public string Source { get; set; }
    }

    public class RightsAndInterests
    {
        public string UserId { get; set; }
        public string AppExpiryDate { get; set; }
        public int VirtualDuration { get; set; }
        public int UsedVirtualDuration { get; set; }
        public object TotalVirtualDuration { get; set; }
        public bool ShowExpire { get; set; }
    }

    public class InviteCodeResponse
    {
        public string Code { get; set; }
    }
}
