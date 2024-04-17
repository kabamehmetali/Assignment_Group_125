using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_125.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
        public decimal Balance { get; set; }  // Add balance field
    }
}
