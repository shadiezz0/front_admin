using Coder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Infrastructure.Data.Configuration
{
    public class CodeAttributeMainConfiguration : IEntityTypeConfiguration<CodeAttributeMain>
    {
        public void Configure(EntityTypeBuilder<CodeAttributeMain> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.NameAr)
                .HasMaxLength(150);

            builder.Property(x => x.NameEn)
                .HasMaxLength(150);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(300);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(300);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.HasOne(x => x.CodeAttributeType)
                .WithMany(x => x.CodeAttributeMains)
                .HasForeignKey(x => x.CodeAttributeTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CodeAttributeDetails)
                .WithOne(x => x.CodeAttributeMain)
                .HasForeignKey(x => x.AttributeMainId);

            builder.HasIndex(x => new { x.CodeAttributeTypeId, x.Code }).IsUnique();

            builder.ToTable("CodeAttributeMain");
        }
    }
}

