using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class CodeTypeSequenceDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public int CodeTypeId { get; set; }
        public int StartWith { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int CurrentValue { get; set; }
        public bool IsCycling { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
