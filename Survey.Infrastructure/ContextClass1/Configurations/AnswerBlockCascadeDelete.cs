namespace Survey.Infrastructure.ContextClass1.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Survey.Infrastructure.Entities;

    public class AnswerBlockCascadeDelete : IEntityTypeConfiguration<AnwserBlock>
    {
        public void Configure(EntityTypeBuilder<AnwserBlock> builder)
        {
            builder.Property(p => p.AnwserBlockID)
                .HasColumnName("AnwserBlockID")
                .IsRequired();

            builder.Property(p => p.CompanyID)
                .HasColumnType("int");

            builder.Property(p => p.CodeOfAnwserBlock)
                .HasColumnType("int");

            builder.Property(p => p.AnwserBlockName)
                .HasColumnType("varchar(255)");

            builder.Property(p => p.BlockType)
               .HasColumnType("varchar(30)");

            builder.HasOne(pm => pm.Company)
                .WithMany(p => p.AnswerBlock)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pm => pm.Anwsers)
                .WithOne(p => p.AnwserBlock)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(pm => pm.Questions)
                .WithOne(p => p.AnwserBlock)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
