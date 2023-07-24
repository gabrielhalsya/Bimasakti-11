using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common.DTO_s
{
    public class REFRESH_CURRENCY_RATE_RESULT : R_APIResultBaseDTO
    {
        public string CCURRENCY_CODE { get; set; }
        public string CRATETYPE_CODE { get; set; }
        public string CRATE_DATE { get; set; }
        public decimal NLBASE_RATE_AMOUNT { get; set; }
        public decimal NLCURRENCY_RATE_AMOUNT { get; set; }
        public decimal NBBASE_RATE_AMOUNT { get; set; }
        public decimal NBCURRENCY_RATE_AMOUNT { get; set; }
    }
}
