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
    }

    public class PricingSaveParamDTO : PricingParamDTO
    {
        public List<PricingBulkSaveDTO> PRICING_LIST { get; set; }
    }
    public class PricingRateSaveParamDTO : PricingParamDTO
    {
        public List<PricingRateBulkSaveDTO> PRICING_RATE_LIST { get; set; }
    }
}