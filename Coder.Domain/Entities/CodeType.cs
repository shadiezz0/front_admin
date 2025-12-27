using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class CodeType
    {
        public int Id { get; set; }
        public string CodeTypeCode { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }

        // Navigation properties
        public virtual ICollection<CodeTypeSetting> CodeTypeSettings { get; set; } = new List<CodeTypeSetting>();
        public virtual ICollection<CodeTypeSequence> CodeTypeSequences { get; set; } = new List<CodeTypeSequence>();
        public virtual ICollection<Code> Codes { get; set; } = new List<Code>();
    }
}

