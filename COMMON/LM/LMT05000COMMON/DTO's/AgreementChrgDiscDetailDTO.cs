using System;
using System.Collections.Generic;
using System.Text;

namespace LMT05000COMMON.DTO_s
{
    public class AgreementChrgDiscDetailDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CREF_NO { get; set; }
        public string CSEQ_NO { get; set; }
        public string CFLOOR_ID { get; set; }
        public string CUNIT_ID { get; set; }
        public string CTENANT_ID { get; set; }
        public decimal NCHARGES_AMOUNT { get; set; }
        public decimal NCHARGE_DISCOUNT { get; set; }
        public decimal NNET_CHARGE { get; set; }
        public string CAGREEMENT_NO { get; set; }
        public string CSTART_DATE { get; set; }
        public string CEND_DATE { get; set; }
        public string CEXISTING_DISCOUNT_CODE { get; set; }
        public decimal NEXISTING_CHARGE_DISCOUNT { get; set; }
    }

}
