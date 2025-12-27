using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class CodeTypeSetting
    {
        public int Id { get; set; }
        public int CodeTypeId { get; set; }
        public int AttributeDetailId { get; set; }
        public int SortOrder { get; set; }
        public string Separator { get; set; } = "-";
        public bool IsRequired { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual CodeType CodeType { get; set; }
        public virtual CodeAttributeDetails CodeAttributeDetails { get; set; }
    }
}
