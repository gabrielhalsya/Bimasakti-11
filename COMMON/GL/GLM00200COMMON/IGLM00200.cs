using GLM00200Common.DTO_s;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace GLM00200Common
{
    public interface IGLM00200 : R_IServiceCRUDBase<JournalParamDTO>
    {
        IAsyncEnumerable<JournalGridDTO> GetAllRecurringList();
        IAsyncEnumerable<JournalGridDTO> GetFilteredRecurringList();
        IAsyncEnumerable<JournalDetailGridDTO> GetAllJournalDetailList();
        VAR_GSM_COMPANY_DTO GetVAR_GSM_COMPANY();
        VAR_GL_SYSTEM_PARAM_DTO GetVAR_GL_SYSTEM_PARAM();
        VAR_CCURRENT_PERIOD_START_DATE_DTO GetCCURRENT_PERIOD_START_DATE();
        VAR_CSOFT_PERIOD_START_DATE_DTO GetCSOFT_PERIOD_START_DATE();
        VAR_IUNDO_COMMIT_JRN_DTO GetIUNDO_COMMIT_JRN();
        VAR_GSM_TRANSACTION_CODE_DTO GetGSM_TRANSACTION_CODE();
        VAR_GSM_PERIOD_DTO GetGSM_PERIOD();
        IAsyncEnumerable<VAR_STATUS_DTO> GetSTATUS_DTO();
        IAsyncEnumerable<VAR_CURRENCY> GetVAR_CURRENCIES();
        REFRESH_CURRENCY_RATE_RESULT RefreshCurrencyRate();
        JournalCommitApprovalRESULT JournalCommitApproval();
    }
}
