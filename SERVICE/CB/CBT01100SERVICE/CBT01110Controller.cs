using CBT01100COMMON;
using Microsoft.AspNetCore.Mvc;

namespace CBT01100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CBT01110Controller : ControllerBase, ICBT01110
    {
        public CBT01100RecordResult<CBT01110DTO> GetJournalRecord(CBT01110DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01110LastCurrencyRateDTO> GetLastCurrency(CBT01110LastCurrencyRateDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01110DTO> SaveJournal(CBT01110HeaderDetailDTO poEntity)
        {
            throw new NotImplementedException();
        }
    }
}
