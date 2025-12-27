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
    public class CodeTypeSequenceConfiguration : IEntityTypeConfiguration<CodeTypeSequence>
    {
        public void Configure(EntityTypeBuilder<CodeTypeSequence> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NameEn)
                .HasMaxLength(100);

            builder.Property(x => x.StartWith)
                .HasDefaultValue(1);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.CodeType)
                .WithMany(x => x.CodeTypeSequences)
                .HasForeignKey(x => x.CodeTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("CodeTypeSequence");
        }
    }
}
