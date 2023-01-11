namespace Survey.Infrastructure.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        //public int RoleId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Company>? Company { get; set; }

        //public Role Role { get; set; }

    }
}
