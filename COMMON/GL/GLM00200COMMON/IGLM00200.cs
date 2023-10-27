using GLM00200Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace GLM00200Common
{
    public interface IGLM00200 : R_IServiceCRUDBase<JournalParamDTO>
    {
        InitResult GetInitData();
        IAsyncEnumerable<StatusDTO> GetStatusList();
        IAsyncEnumerable<CurrencyDTO> GetCurrencyList();
        IAsyncEnumerable<JournalGridDTO> GetAllRecurringList();
        IAsyncEnumerable<JournalGridDTO> GetFilteredRecurringList();
        IAsyncEnumerable<JournalDetailGridDTO> GetAllJournalDetailList();
        IAsyncEnumerable<JournalDetailActualGridDTO> GetAllActualJournalDetailList();
        CurrencyRateResult RefreshCurrencyRate();
        JournalCommitApprovalRESULT JournalCommitApproval();
    }
}
