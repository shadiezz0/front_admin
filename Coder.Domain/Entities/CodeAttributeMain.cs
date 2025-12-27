using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class CodeAttributeMain
    {
        public int Id { get; set; }
        public int CodeTypeId { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int CodeAttributeTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        // Navigation properties
        public virtual CodeAttributeType CodeAttributeType { get; set; }
        public virtual ICollection<CodeAttributeDetails> CodeAttributeDetails { get; set; } = new List<CodeAttributeDetails>();
    }
}
