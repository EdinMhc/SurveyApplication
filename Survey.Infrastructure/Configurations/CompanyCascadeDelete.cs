using Microsoft.EntityFrameworkCore;
using Survey.Infrastructure.Entities;

namespace Survey.Infrastructure.Configurations
{

    public class CompanyCascadeDelete : IEntityTypeConfiguration<Company>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Company> builder)
        {
            builder.Property(p => p.CompanyID)
                .HasColumnName("CompanyID")
                .IsRequired();

            builder.Property(p => p.CompanyName)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Address)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Email)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.CreateDate)
               .HasColumnType("datetime");

            builder.HasMany(pm => pm.Surveys)
                .WithOne(p => p.Company)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
