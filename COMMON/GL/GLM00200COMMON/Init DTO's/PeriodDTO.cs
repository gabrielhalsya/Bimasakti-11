using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common.Init_DTO_s
{
    public class PeriodDTO : R_APIResultBaseDTO
    {

        public int IMIN_YEAR { get; set; }
        public int IMAX_YEAR { get; set; }
    }

    public class PeriodFrontDTO
    {
        public string CPERIOD_MM_CODE { get; set; }
        public string CPERIOD_MM_TEXT { get; set; }
    }
}
