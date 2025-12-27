using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class CodeTypeSequence
    {

        public int Id { get; set; }
        public string NameEn { get; set; }
        public int CodeTypeId { get; set; }
        public int StartWith { get; set; } //0
        public int MinValue { get; set; } //000 
        public int MaxValue { get; set; } //000123
        public int CurrentValue { get; set; } //3 4 5 6 
        public bool IsCycling { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual CodeType CodeType { get; set; }
    }
}