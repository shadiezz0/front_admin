using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class UpdateCodeTypeSettingDto
    {
        public int SortOrder { get; set; }
        public string Separator { get; set; }
        public bool IsRequired { get; set; }
    }
}
