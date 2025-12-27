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
    public class CodeAttributeTypeConfiguration : IEntityTypeConfiguration<CodeAttributeType>
    {
        public void Configure(EntityTypeBuilder<CodeAttributeType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameAr)
                .HasMaxLength(100);

            builder.Property(x => x.NameEn)
                .HasMaxLength(100);

            builder.Property(x => x.DescriptionAr)
                .HasMaxLength(300);

            builder.Property(x => x.DescriptionEn)
                .HasMaxLength(300);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(100);

            builder.HasMany(x => x.CodeAttributeMains)
                .WithOne(x => x.CodeAttributeType)
                .HasForeignKey(x => x.CodeAttributeTypeId);

            builder.ToTable("CodeAttributeType");
        }
    }
}
