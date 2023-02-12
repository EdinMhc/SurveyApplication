namespace Survey.Infrastructure.Entities
{

    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Company>? Company { get; set; }
    }
}
