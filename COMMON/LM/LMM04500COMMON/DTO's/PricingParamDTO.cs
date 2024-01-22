using LMM04500COMMON.DTO_s;

namespace LMM04500COMMON
{
    public class PricingParamDTO : PricingDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CUNIT_TYPE_CATEGORY_ID { get; set; }
        public string CVALID_INTERNAL_ID { get; set; }
        public string CVALID_DATE { get; set; }
        public string CACTION { get; set; }
    }
}