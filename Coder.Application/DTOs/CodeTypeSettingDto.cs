using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class CodeTypeSettingDto
    {
        public int Id { get; set; }
        public int CodeTypeId { get; set; }
        public int AttributeDetailId { get; set; }
        public int SortOrder { get; set; }
        public string Separator { get; set; }
        public bool IsRequired { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
