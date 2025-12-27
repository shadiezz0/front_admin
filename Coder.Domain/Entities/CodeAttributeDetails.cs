using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class CodeAttributeDetails
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int AttributeMainId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int? ParentDetailId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }

        // Navigation properties
        public virtual CodeAttributeMain CodeAttributeMain { get; set; }
        public virtual CodeAttributeDetails ParentDetail { get; set; }
        public virtual ICollection<CodeAttributeDetails> ChildDetails { get; set; } = new List<CodeAttributeDetails>();
        public virtual ICollection<CodeTypeSetting> CodeTypeSettings { get; set; } = new List<CodeTypeSetting>();
    }
}
