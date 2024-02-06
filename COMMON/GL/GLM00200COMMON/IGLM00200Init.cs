using System;
using System.Collections.Generic;

namespace GLM00200COMMON
{
    public interface IGLM00200Init
    {
        #region Init
        GLM00200RecordResult<GLM00200GSCompanyInfoDTO> GetGSCompanyInfo();
        GLM00200RecordResult<GLM00200GLSystemParamDTO> GetGLSystemParam();
        GLM00200RecordResult<GLM00200GSPeriodDTInfoDTO> GetGSPeriodDTInfo(GLM00200ParamGSPeriodDTInfoDTO poEntity);
        GLM00200RecordResult<GLM00200GLSystemEnableOptionInfoDTO> GetGSSystemEnableOptionInfo();
        GLM00200RecordResult<GLM00200GSTransInfoDTO> GetGSTransCodeInfo();
        GLM00200RecordResult<GLM00200GSPeriodYearRangeDTO> GetGSPeriodYearRange();
        GLM00200RecordResult<GLM00200TodayDateDTO> GetTodayDate();
        IAsyncEnumerable<GLM00200GSGSBCodeDTO> GetGSBCodeList();
        IAsyncEnumerable<GLM00200GSCurrencyDTO> GetCurrencyList();
        IAsyncEnumerable<GLM00200GSCenterDTO> GetCenterList();
        #endregion

        GLM00200RecordResult<GLM00200InitDTO> GetTabJournalListInitVar();
        GLM00200RecordResult<GLM00210InitDTO> GetTabJournalEntryInitVar();
    }
}
