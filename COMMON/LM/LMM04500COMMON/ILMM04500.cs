﻿using LMM04500COMMON.DTO_s;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMM04500COMMON
{
    public interface ILMM04500 : R_IServiceCRUDBase<PricingDTO>
    {
        IAsyncEnumerable<PropertyDTO> GetPropertyList();
        IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList();
        IAsyncEnumerable<PricingDTO> GetPricingList();
        IAsyncEnumerable<PricingDTO> GetPricingDateList();

    }
}
