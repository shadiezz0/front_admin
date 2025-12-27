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
    public class CodeConfiguration : IEntityTypeConfiguration<Code>
    {
        public void Configure(EntityTypeBuilder<Code> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameAr)
                .HasMaxLength(200);

            builder.Property(x => x.NameEn)
                .HasMaxLength(200);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(500);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(500);

            builder.Property(x => x.CodeGenerated)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Status)
                .HasMaxLength(30)
                .HasDefaultValue("DRAFT");

            builder.Property(x => x.ExternalSystem)
                .HasMaxLength(50);

            builder.Property(x => x.ExternalReferenceId)
                .HasMaxLength(100);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.ApprovedBy)
                .HasMaxLength(100);

            builder.HasOne(x => x.CodeType)
                .WithMany(x => x.Codes)
                .HasForeignKey(x => x.CodeTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CodeTypeId, x.CodeGenerated }).IsUnique();

            builder.ToTable("Code");
        }
    }
}

