using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common
{
    public class JournalGridDTO :JournalDTO
    {
        public string LALLOW_APPROVE { get; set; }
        public string CNEXT_PRD { get; set; }
        public  DateTime DSTART_DATE { get; set; }
        public decimal NTRANS_AMOUNT { get; set; }
        public string CSTATUS_NAME { get; set; }
    }
}
