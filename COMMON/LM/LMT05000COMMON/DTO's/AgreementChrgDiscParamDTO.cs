using System;
using System.Collections.Generic;
using System.Text;

namespace LMT05000COMMON.DTO_s
{
    public class AgreementChrgDiscParamDTO : AgreementChrgDiscHeaderDTO
    {
        public string CDISCOUNT_CODE { get; set; }
        public List<AgreementChrgDiscDetailDTO> AgreementChrgDiscDetail { get; set; }
    }
}
