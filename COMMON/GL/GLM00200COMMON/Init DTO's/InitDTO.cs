using GLM00200Common.Init_DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common
{
    public class InitDTO
    {
        public DateTime DTODAY { get; set; }
        public PeriodDetailInfoDTO CURRENT_PERIOD_START_DATE { get; set; }
        public PeriodDetailInfoDTO SOFT_PERIOD_START_DATE { get; set; }
        public CurrencyDTO CURRENCY { get; set; }
        public SystemParamDTO SYSTEM_PARAM { get; set; }
        public CompanyDTO COMPANY_INFO { get; set; }
        public PeriodDTO PERIOD_YEAR { get; set; }
        public TransCodeDTO TRANSACTION_CODE { get; set; }
        public int IOPTON { get; set; }
    }
}
