namespace Survey.Infrastructure.ContextClass1.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Survey.Infrastructure.Entities;

    public class AswerCascadeDelete : IEntityTypeConfiguration<Anwser>
    {
        public void Configure(EntityTypeBuilder<Anwser> builder)
        {
            builder.Property(p => p.AnwserID)
                .HasColumnName("AnwserID")
                .IsRequired();

            builder.Property(p => p.AnwserBlockID)
                .HasColumnType("int");

            builder.Property(p => p.AnwserText)
                .HasColumnType("varchar(100)");

            builder.HasOne(pm => pm.AnwserBlock)
                .WithMany(p => p.Anwsers)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
