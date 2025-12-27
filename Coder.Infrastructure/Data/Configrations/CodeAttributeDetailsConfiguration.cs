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
    internal class CodeAttributeDetailsConfiguration : IEntityTypeConfiguration<CodeAttributeDetails>
    {
        public void Configure(EntityTypeBuilder<CodeAttributeDetails> builder)
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

            builder.HasOne(x => x.CodeAttributeMain)
                .WithMany(x => x.CodeAttributeDetails)
                .HasForeignKey(x => x.AttributeMainId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ParentDetail)
                .WithMany(x => x.ChildDetails)
                .HasForeignKey(x => x.ParentDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CodeTypeSettings)
                .WithOne(x => x.CodeAttributeDetails)
                .HasForeignKey(x => x.AttributeDetailId);

            builder.HasIndex(x => new { x.AttributeMainId, x.Code }).IsUnique();

            builder.ToTable("CodeAttributeDetails");
        }
    }
}

