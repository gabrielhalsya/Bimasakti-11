using CBT01100COMMON;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBT01100MODEL
{
    public class CBT01110Model : R_BusinessObjectServiceClientBase<CBT01110DTO>, ICBT01110
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlCB";
        private const string DEFAULT_ENDPOINT = "api/CBT01110";
        private const string DEFAULT_MODULE = "CB";

        public CBT01110Model
            (
                string pcHttpClientName = DEFAULT_HTTP,
                string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
                bool plSendWithContext = true,
                bool plSendWithToken = true
            )
        : base
            (
                pcHttpClientName,
                pcRequestServiceEndPoint,
                DEFAULT_MODULE,
                plSendWithContext,
                plSendWithToken
            )
        { }

        public async Task<CBT01110DTO> GetJournalRecordAsync(CBT01110DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01110DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01100RecordResult<CBT01110DTO>, CBT01110DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01110.GetJournalRecord),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01110LastCurrencyRateDTO> GetLastCurrencyAsync(CBT01110LastCurrencyRateDTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01110LastCurrencyRateDTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01100RecordResult<CBT01110LastCurrencyRateDTO>, CBT01110LastCurrencyRateDTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01110.GetLastCurrency),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01110DTO> SaveJournalAsync(CBT01110HeaderDetailDTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01110DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01100RecordResult<CBT01110DTO>, CBT01110HeaderDetailDTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01110.SaveJournal),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01110DTO> SaveJournalDetailAsync(CBT01111DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01110DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01100RecordResult<CBT01110DTO>, CBT01111DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01110.SaveJournalDetail),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        #region Not Implement

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

        public CBT01100RecordResult<CBT01110DTO> SaveJournalDetail(CBT01111DTO poEntity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
