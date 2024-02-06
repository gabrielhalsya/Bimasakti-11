using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace GLM00200COMMON
{
    public interface IGLM00210 
    {
        GLM00200RecordResult<GLM00210LastCurrencyRateDTO> GetLastCurrency(GLM00210LastCurrencyRateDTO poEntity);
        GLM00200RecordResult<GLM00210DTO> GetJournalRecord(GLM00210DTO poEntity);
        GLM00200RecordResult<GLM00210DTO> SaveJournal(GLM00210HeaderDetailDTO poEntity);
    }
}
