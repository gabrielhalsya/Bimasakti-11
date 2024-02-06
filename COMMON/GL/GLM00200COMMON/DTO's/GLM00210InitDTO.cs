using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200COMMON
{
    public class GLM00210InitDTO
    {
        public GLM00200GSTransInfoDTO VAR_GSM_TRANSACTION_CODE { get; set; }
        public GLM00200TodayDateDTO VAR_TODAY { get; set; }
        public GLM00200GSCompanyInfoDTO VAR_GSM_COMPANY { get; set; }
        public GLM00200GLSystemEnableOptionInfoDTO VAR_IUNDO_COMMIT_JRN { get; set; }
        public GLM00200GLSystemParamDTO VAR_GL_SYSTEM_PARAM { get; set; }
        public List<GLM00200GSCurrencyDTO> VAR_CURRENCY_LIST { get; set; }
        public List<GLM00200GSCenterDTO> VAR_CENTER_LIST { get; set; }  
        public List<GLM00200GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; }
    }
}
