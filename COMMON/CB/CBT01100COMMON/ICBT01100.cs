using System;
using System.Collections.Generic;

namespace CBT01100COMMON
{
    public interface ICBT01100
    {
        IAsyncEnumerable<CBT01100DTO> GetJournalList();
        IAsyncEnumerable<CBT01100DTO> GetJournalDetailList();
        CBT01100RecordResult<CBT01100UpdateStatusDTO> UpdateJournalStatus(CBT01100UpdateStatusDTO poEntity);
        CBT01100RecordResult<CBT01100RapidApprovalValidationDTO> ValidationRapidApproval(CBT01100RapidApprovalValidationDTO poEntity);
    }
}
