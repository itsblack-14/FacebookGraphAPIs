namespace FacebookGraphAPIs.Features.FacebookLogin
{
    public class FacebookLoginResponse : ResponseStatus
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FacebookId { get; set; }
        public string Token { get; set; }
    }
}
