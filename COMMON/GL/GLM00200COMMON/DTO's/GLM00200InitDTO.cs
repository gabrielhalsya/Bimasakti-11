using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200COMMON
{
    public class GLM00200InitDTO
    {
        public GLM00200GLSystemParamDTO VAR_GL_SYSTEM_PARAM { get; set; }
        public List<GLM00200GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; }
        public GLM00200GSTransInfoDTO VAR_GSM_TRANSACTION_CODE { get; set; }
        public GLM00200GLSystemEnableOptionInfoDTO VAR_IUNDO_COMMIT_JRN { get; set; }
        public GLM00200GSPeriodYearRangeDTO VAR_GSM_PERIOD { get; set; }
    }
}
