using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class CreateCodeAttributeDetailsDto
    {
        public string Code { get; set; }
        public int AttributeMainId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int? ParentDetailId { get; set; }
        public int sortOrder { get; set; }
    }
}
