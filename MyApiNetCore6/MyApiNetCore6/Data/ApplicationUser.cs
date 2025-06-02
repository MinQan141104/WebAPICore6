using Microsoft.AspNetCore.Identity;

namespace MyApiNetCore6.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
