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
    public class CodeTypeConfiguration : IEntityTypeConfiguration<CodeType>
    {
        public void Configure(EntityTypeBuilder<CodeType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CodeTypeCode)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.NameAr)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.NameEn)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(300);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(300);

          

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.Property(x => x.ApprovedBy)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.ApprovedAt)
                .IsRequired(false);

            builder.HasIndex(x => x.CodeTypeCode).IsUnique();

            builder.ToTable("CodeType");
        }
    }
}
