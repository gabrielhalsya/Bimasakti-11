using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200Common
{

    public class InitResult : R_APIResultBaseDTO
    {
        public InitResultData data { get;set; }
    }
    public class InitResultData
    {
        public TodayDTO OTODAY { get; set; }
        public PeriodDetailInfoDTO OCURRENT_PERIOD_START_DATE { get; set; }
        public PeriodDetailInfoDTO OSOFT_PERIOD_START_DATE { get; set; }
        public CurrencyDTO OCURRENCY { get; set; }
        public SystemParamDTO OGL_SYSTEM_PARAM { get; set; }
        public CompanyDTO OGSM_COMPANY { get; set; }
        public GSM_PeriodDTO OGSM_PERIOD { get; set; }
        public TransCodeDTO OGSM_TRANSACTION_CODE { get; set; }
        public UndoCommitJrnDTO OOUNDO_COMMIT_JRN { get; set; }
    }


}
