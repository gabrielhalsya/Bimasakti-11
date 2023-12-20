using LMM01500Common.DTO;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;

namespace LMM01500Common
{
    public interface ILMM01500 : R_IServiceCRUDBase<InvoiceGroupDTO>
    {
        IAsyncEnumerable<InvoiceGroupDTO> GetInvoiceGroupList();
    }
}
