using LMT05000COMMON.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMT05000COMMON
{
    public interface ILMM05000Init
    {
        IAsyncEnumerable<PropertyDTO> GetProperty();
        IAsyncEnumerable<GSB_CodeInfoDTO> GetGSBCodeInfo();
        IAsyncEnumerable<GSPeriodDT_DTO> GetGSPeriodDT();
    }
}
