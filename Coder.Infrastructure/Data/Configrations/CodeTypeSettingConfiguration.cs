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
    public class CodeTypeSettingConfiguration : IEntityTypeConfiguration<CodeTypeSetting>
    {
        public void Configure(EntityTypeBuilder<CodeTypeSetting> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Separator)
                .HasMaxLength(5)
                .HasDefaultValue("-");

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.CodeType)
                .WithMany(x => x.CodeTypeSettings)
                .HasForeignKey(x => x.CodeTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.CodeAttributeDetails)
                .WithMany(x => x.CodeTypeSettings)
                .HasForeignKey(x => x.AttributeDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CodeTypeId, x.AttributeDetailId }).IsUnique();

            builder.ToTable("CodeTypeSetting");
        }
    }
}
