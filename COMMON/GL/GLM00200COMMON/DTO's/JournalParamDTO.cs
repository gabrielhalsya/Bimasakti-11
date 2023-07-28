using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common.DTO_s
{
    public class JournalParamDTO : JournalDTO
    {
        public List<JournalDetailGridDTO> ListJournalDetail { get; set; }
    }
}
