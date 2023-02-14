using Survey.Infrastructure.Entities;

namespace Survey.Infrastructure.Configurations
{

    public class SurveyCascadeDelete : IEntityTypeConfiguration<Surveys>
    {
        public void Configure(EntityTypeBuilder<Surveys> builder)
        {
            builder.Property(p => p.SurveyID).HasColumnName("SurveyID");

            builder.Property(p => p.IsActive)
                .HasColumnType("bit")
                .HasMaxLength(255);

            builder.Property(p => p.CreatedBy)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);

            builder.Property(p => p.CreateDate)
                .HasColumnType("datetime");

            builder.Property(p => p.SurveyName)
               .HasColumnType("varchar(255)")
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.CompanyID)
               .HasColumnType("int")
               .IsRequired();

            builder.HasOne(pm => pm.Company)
                .WithMany(p => p.Surveys)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(pm => pm.SurveyReport)
                .WithOne(p => p.Survey)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pm => pm.Questions)
                .WithOne(p => p.Survey)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
