using CBT01100COMMON;
using Microsoft.AspNetCore.Mvc;

namespace CBT01100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CBT01100InitController : ControllerBase, ICBT01100Init
    {
        public IAsyncEnumerable<CBT01100GSCenterDTO> GetCenterList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CBT01100GSCurrencyDTO> GetCurrencyList()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GLSystemParamDTO> GetGLSystemParam()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CBT01100GSGSBCodeDTO> GetGSBCodeList()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GSCompanyInfoDTO> GetGSCompanyInfo()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GSPeriodDTInfoDTO> GetGSPeriodDTInfo(CBT01100ParamGSPeriodDTInfoDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GSPeriodYearRangeDTO> GetGSPeriodYearRange()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GLSystemEnableOptionInfoDTO> GetGSSystemEnableOptionInfo()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100GSTransInfoDTO> GetGSTransCodeInfo()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01110InitDTO> GetTabJournalEntryUniversalVar()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100InitDTO> GetTabJournalListUniversalVar()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100TodayDateDTO> GetTodayDate()
        {
            throw new NotImplementedException();
        }
    }
}
