using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.DTOs
{
    public class CreateCodeDto
    {
        public int CodeTypeId { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public string ExternalSystem { get; set; }

        public string ExternalReferenceId { get; set; }
    }
}