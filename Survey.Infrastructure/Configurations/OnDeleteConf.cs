using Microsoft.EntityFrameworkCore;
using Survey.Infrastructure.Entities;

namespace Survey.Infrastructure.Configurations
{

    public class OnDeleteConf : IEntityTypeConfiguration<SurveyReportData>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<SurveyReportData> builder)
        {
            builder.Property(p => p.RespondentID).HasColumnName("RespondentId");

            builder.Property(p => p.QuestionID)
                .HasColumnType("int")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.AnswerID)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.SurveyReportID)
               .HasColumnType("int")
               .IsRequired();

            builder.HasOne(pm => pm.SurveyReport)
                .WithMany(p => p.SurveyReportData)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}