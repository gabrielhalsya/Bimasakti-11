using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GLM00200COMMON
{
    public class GLM00210HeaderDetailDTO
    {
        public GLM00210DTO HeaderData { get; set; }
        public List<GLM00211DTO> DetailData { get; set; }
    }
}
