using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class CodeDto
    {
        public int Id { get; set; }
        public int CodeTypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string CodeGenerated { get; set; }
        public string Status { get; set; }
        public string ExternalSystem { get; set; }
        public string ExternalReferenceId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string ApprovedBy { get; set; }
    }
}
