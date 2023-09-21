using System;
using System.Collections.Generic;

namespace GLT00100Common
{
    public class GLT00100DTO
    {
        public string CLANGUAGE_ID { get; set; }
        public string CCOMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
        public string CACTION { get; set; }
        public string CREC_ID { get; set; } = "";
        public bool LALLOW_APPROVE { get; set; }
        public string CDEPT_CODE { get; set; }
        public string CDEPT_NAME { get; set; }
        public string CREF_NO { get; set; }
        public string CREF_DATE { get; set; }
        public DateTime DREF_DATE { get; set; }
        public string CDOC_NO { get; set; }
        public string CDOC_DATE { get; set; }
        public DateTime DDOC_DATE { get; set; }
        public string CREF_PRD { get; set; }
        public string CTRANS_CODE { get; set; }
        public string CTRANS_DESC { get; set; }
        public string CCURRENCY_CODE { get; set; }
        public Decimal NLBASE_RATE { get; set; }
        public Decimal NLCURRENCY_RATE { get; set; }
        public string CLOCAL_CURRENCY_CODE { get; set; }
        public Decimal NBBASE_RATE { get; set; }
        public Decimal NBCURRENCY_RATE { get; set; }
        public string CBASE_CURRENCY_CODE { get; set; }
        public Decimal NPRELIST_AMOUNT { get; set; }
        public Decimal NCREDIT_AMOUNT { get; set; }
        public Decimal NDEBIT_AMOUNT { get; set; }
        public string CSTATUS_NAME { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }


        public List<GLT00100DetailDTO> LIST_JOURNAL_DETAIL { get; set; }
    }

}
