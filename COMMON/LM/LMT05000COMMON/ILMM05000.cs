using LMT05000COMMON.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMT05000COMMON
{
    public interface ILMM05000
    {
        IAsyncEnumerable<AgreementChrgDiscDetailDTO> GetAgreementChargesDiscountList(AgreementChrgDiscParamDTO poParam);
        AgreementChrgDiscResultDTO ProcessAgreementChargeDiscount(AgreementChrgDiscParamDTO popaParam);

    }
}
