using CBT01100COMMON;
using CBT01100COMMON.DTO_s;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBT01100MODEL
{
    public class CBT01100Model : R_BusinessObjectServiceClientBase<CBT01100DTO>, ICBT01100
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlCB";
        private const string DEFAULT_ENDPOINT = "api/CBT01100";
        private const string DEFAULT_MODULE = "CB";

        public CBT01100Model(string pcHttpClientName = DEFAULT_HTTP,
                             string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
                             bool plSendWithContext = true,
                             bool plSendWithToken = true) : base(
                                 pcHttpClientName,
                                 pcRequestServiceEndPoint,
                                 DEFAULT_MODULE,
                                 plSendWithContext,
                                 plSendWithToken)
        { }

        public async Task<List<CBT01100DTO>> GetJournalListAsync(CBT01100ParamDTO poEntity)
        {
            var loEx = new R_Exception();
            List<CBT01100DTO> loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantCBT01100.CDEPT_CODE, poEntity.CDEPT_CODE);
                R_FrontContext.R_SetStreamingContext(ContextConstantCBT01100.CPERIOD, poEntity.CPERIOD);
                R_FrontContext.R_SetStreamingContext(ContextConstantCBT01100.CSTATUS, string.IsNullOrWhiteSpace(poEntity.CSTATUS) ? "" : poEntity.CSTATUS);
                R_FrontContext.R_SetStreamingContext(ContextConstantCBT01100.CSEARCH_TEXT, string.IsNullOrWhiteSpace(poEntity.CSEARCH_TEXT) ? "" : poEntity.CSEARCH_TEXT);


                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CBT01100DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01100.GetJournalList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task<List<CBT01101DTO>> GetJournalDetailListAsync(CBT01101DTO poEntity)
        {
            var loEx = new R_Exception();
            List<CBT01101DTO> loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantCBT01100.CREC_ID, poEntity.CREC_ID);


                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CBT01101DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01100.GetJournalDetailList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task UpdateJournalStatusAsync(CBT01100UpdateStatusDTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01100RecordResult<CBT01100UpdateStatusDTO>, CBT01100UpdateStatusDTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01100.UpdateJournalStatus),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Not Implement

        public CBT01100RecordResult<CBT01100UpdateStatusDTO> UpdateJournalStatus(CBT01100UpdateStatusDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CBT01101DTO> GetJournalDetailList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CBT01100DTO> GetJournalList()
        {
            throw new NotImplementedException();
        }

        public CBT01100RecordResult<CBT01100RapidApprovalValidationDTO> ValidationRapidApproval(CBT01100RapidApprovalValidationDTO poEntity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
