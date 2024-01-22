using LMM04500COMMON.DTO_s;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMM04500COMMON
{
    public interface ILMM04501 : R_IServiceCRUDBase<PricingRateDTO>
    {
        IAsyncEnumerable<PricingRateDTO> GetPricingRateList();

    }
}
