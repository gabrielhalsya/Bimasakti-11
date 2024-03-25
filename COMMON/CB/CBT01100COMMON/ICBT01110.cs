using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace CBT01100COMMON
{
    public interface ICBT01110 
    {
        CBT01100RecordResult<CBT01110LastCurrencyRateDTO> GetLastCurrency(CBT01110LastCurrencyRateDTO poEntity);
        CBT01100RecordResult<CBT01110DTO> GetJournalRecord(CBT01110DTO poEntity);
        CBT01100RecordResult<CBT01110DTO> SaveJournal(CBT01110HeaderDetailDTO poEntity);
    }
}
