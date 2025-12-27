using Coder.Domain.Entities;
using Coder.Domain.Interfaces;
using Coder.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IGenericRepository<CodeType> _codeTypes;
        private IGenericRepository<CodeAttributeType> _codeAttributeTypes;
        private IGenericRepository<CodeAttributeMain> _codeAttributeMains;
        private IGenericRepository<CodeAttributeDetails> _codeAttributeDetails;
        private IGenericRepository<CodeTypeSetting> _codeTypeSettings;
        private IGenericRepository<CodeTypeSequence> _codeTypeSequences;
        private IGenericRepository<Code> _codes;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<CodeType> CodeTypes
            => _codeTypes ??= new GenericRepository<CodeType>(_context);

        public IGenericRepository<CodeAttributeType> CodeAttributeTypes
            => _codeAttributeTypes ??= new GenericRepository<CodeAttributeType>(_context);

        public IGenericRepository<CodeAttributeMain> CodeAttributeMains
            => _codeAttributeMains ??= new GenericRepository<CodeAttributeMain>(_context);

        public IGenericRepository<CodeAttributeDetails> CodeAttributeDetails
            => _codeAttributeDetails ??= new GenericRepository<CodeAttributeDetails>(_context);

        public IGenericRepository<CodeTypeSetting> CodeTypeSettings
            => _codeTypeSettings ??= new GenericRepository<CodeTypeSetting>(_context);

        public IGenericRepository<CodeTypeSequence> CodeTypeSequences
            => _codeTypeSequences ??= new GenericRepository<CodeTypeSequence>(_context);

        public IGenericRepository<Code> Codes
            => _codes ??= new GenericRepository<Code>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> BeginTransactionAsync()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CommitTransactionAsync()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RollbackTransactionAsync()
        {
            try
            {
                await _context.Database.RollbackTransactionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
    