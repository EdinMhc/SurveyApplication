namespace Survey.Infrastructure.ContextClass1.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Survey.Infrastructure.Entities;

    public class QuestionCascadeDelete : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(p => p.QuestionID).HasColumnName("QuestionID");

            builder.Property(p => p.QuestionText)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.QuestionType)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.AnwserBlockID)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(p => p.SurveyID)
               .HasColumnType("int")
               .IsRequired();

            builder.Property(p => p.Code)
               .HasColumnType("varchar(255)")
               .IsRequired();

            builder.HasOne(pm => pm.AnwserBlock)
                .WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(pm => pm.Survey)
                .WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
