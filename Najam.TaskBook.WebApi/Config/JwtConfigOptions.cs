namespace Najam.TaskBook.WebApi.Config
{
    public class JwtConfigOptions
    {
        public string SecurityKey { get; set; } = "5FF831CA-7307-49A3-9CE4-806355E3EC78";

        public string Issuer { get; set; } = "default.najam.co.uk";

        public string Audience { get; set; } = "default.najam.co.uk";

        public int ExpiresMinutes { get; set; } = 30;
    }
}
