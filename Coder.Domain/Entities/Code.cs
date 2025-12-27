using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Domain.Entities
{
    public class Code
    {
        public int Id { get; set; }
        public int CodeTypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string CodeGenerated { get; set; }
        public string Status { get; set; } = "DRAFT"; // DRAFT / APPROVED / INACTIVE
        public string ExternalSystem { get; set; } // HMS / ERP
        public string ExternalReferenceId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }

        // Navigation properties
        public virtual CodeType CodeType { get; set; }
    }
}
