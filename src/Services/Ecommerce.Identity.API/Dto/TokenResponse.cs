namespace Ecommerce.Identity.API.Dto
{
    public class TokenDto
    {
        public string jwtToken { get; set; }
        public DateTime ValidTill { get; set; }
    }
}
