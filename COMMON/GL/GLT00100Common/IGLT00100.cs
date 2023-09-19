using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GLT00100Common
{
    public interface IGLT00100 : R_IServiceCRUDBase<GLT00100DTO>
    {
        IAsyncEnumerable<GLT00100GridDTO> GetJournalList();
        IAsyncEnumerable<GLT00100DetailDTO> GetAllJournalDetailList();
    }
}
