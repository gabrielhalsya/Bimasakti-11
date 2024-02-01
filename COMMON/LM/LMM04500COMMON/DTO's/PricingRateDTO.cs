using LMM04500COMMON.DTO_s;
using System;
using System.Globalization;

namespace LMM04500COMMON
{
    public class PricingRateDTO : PricingDTO
    {
        public string CRATE_DATE { get; set; }
        public DateTime DRATE_DATE => CRATE_DATE != null ? DateTime.ParseExact(CRATE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture) : DateTime.Now;
        public string CCURRENCY_CODE { get; set; }
        public decimal NBASE_RATE_AMOUNT { get; set; }
        public decimal NCURRENCY_RATE_AMOUNT { get; set; }
    }
}