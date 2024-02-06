using GLM00200COMMON;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace GLM00200COMMON
{
    public interface IGLM00200 
    {
        IAsyncEnumerable<GLM00200DTO> GetJournalList();
        IAsyncEnumerable<GLM00201DTO> GetJournalDetailList();
        GLM00200RecordResult<GLM00200UpdateStatusDTO> UpdateJournalStatus(GLM00200UpdateStatusDTO poEntity);
        GLM00200RecordResult<GLM00200RapidApprovalValidationDTO> ValidationRapidApproval(GLM00200RapidApprovalValidationDTO poEntity);
    }
}
