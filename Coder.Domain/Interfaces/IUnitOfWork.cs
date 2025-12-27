using Coder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<CodeType> CodeTypes { get; }
        IGenericRepository<CodeAttributeType> CodeAttributeTypes { get; }
        IGenericRepository<CodeAttributeMain> CodeAttributeMains { get; }
        IGenericRepository<CodeAttributeDetails> CodeAttributeDetails { get; }
        IGenericRepository<CodeTypeSetting> CodeTypeSettings { get; }
        IGenericRepository<CodeTypeSequence> CodeTypeSequences { get; }
        IGenericRepository<Code> Codes { get; }

        Task<int> SaveChangesAsync();
        Task<bool> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task<bool> RollbackTransactionAsync();
    }
}
