using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common
{
    public class JournalGridDTO
    {
        public string CREC_ID { get; set; }
        public string LALLOW_APPROVE { get; set; }
        public string CNEXT_PRD { get; set; }
        public string CSTATUS { get; set; }
        public string CREF_NO { get; set; }
        public int IFREQUENCY { get; set; }
        public int IPERIOD { get; set; }
        public string CSTART_DATE { get; set; }
        public  DateTime DSTART_DATE { get; set; }
        public string CNEXT_DATE { get; set; }
        public  DateTime DNEXT_DATE { get; set; }
        public string CTRANS_DESC { get; set; }
        public string CCURRENCY_CODE { get; set; }
        public decimal NTRANS_AMOUNT { get; set; }
        public string CSTATUS_NAME { get; set; }
        public string CDOC_NO { get; set; }
        public string CDOC_DATE { get; set; }
        public DateTime DDOC_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
    }
}
