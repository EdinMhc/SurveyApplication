namespace Survey.Infrastructure.Configurations
{

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin",
            },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin",
            });
        }
    }
}
