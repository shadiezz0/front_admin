using Coder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Infrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CodeType> CodeTypes { get; set; }
        public DbSet<CodeAttributeType> CodeAttributeTypes { get; set; }
        public DbSet<CodeAttributeMain> CodeAttributeMains { get; set; }
        public DbSet<CodeAttributeDetails> CodeAttributeDetails { get; set; }
        public DbSet<CodeTypeSetting> CodeTypeSettings { get; set; }
        public DbSet<CodeTypeSequence> CodeTypeSequences { get; set; }
        public DbSet<Code> Codes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
