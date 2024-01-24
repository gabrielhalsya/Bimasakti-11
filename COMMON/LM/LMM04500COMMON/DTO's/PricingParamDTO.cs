using LMM04500COMMON.DTO_s;
using System.Collections.Generic;

namespace LMM04500COMMON
{
    public class PricingParamDTO : PricingDTO
    {
        public string CUSER_ID { get; set; }
        public string CACTION { get; set; }
        public string CTYPE { get; set; }
        public string CVALID_FROM_DATE { get; set; }
        public List<PricingBulkDTO> PRICING_LIST { get; set; }
    }
}