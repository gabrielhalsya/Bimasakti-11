using GLM00200Common;
using GLM00200Common.DTO_s;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GLM00200Model
{
    public class GLM00200Model : R_BusinessObjectServiceClientBase<JournalParamDTO>, IGLM00200
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlGL";
        private const string DEFAULT_CHECKPOINT_NAME = "api/GLM00200";
        private const string DEFAULT_MODULE = "GL";
        public GLM00200Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_CHECKPOINT_NAME,
            bool plSendWithContext = true,
            bool plSendWithToken = true
            ) : base(
                pcHttpClientName,
                pcRequestServiceEndPoint,
                DEFAULT_MODULE,
                plSendWithContext,
                plSendWithToken)
        {
        }

        //FUNCTION
        #region real function
        public async Task<InitResult> GetInitDataAsync()
        {
            var loEx = new R_Exception();
            InitResult loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<InitResult>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetInitData),
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
        public async Task<List<StatusDTO>> GetStatusListAsync()
        {
            var loEx = new R_Exception();
            List<StatusDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<StatusDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetStatusList),
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
        public async Task<List<CurrencyDTO>> GetCurrencyListAsync()
        {
            var loEx = new R_Exception();
            List<CurrencyDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CurrencyDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetCurrencyList),
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
        public async Task<List<JournalGridDTO>> GetAllRecurringListAsync()
        {
            R_Exception loEx = new R_Exception();
            List<JournalGridDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<JournalGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetAllRecurringList),
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
        public async Task<List<JournalDetailGridDTO>> GetAllJournalDetailListAsync()
        {
            R_Exception loEx = new R_Exception();
            List<JournalDetailGridDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<JournalDetailGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetAllJournalDetailList),
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
        public async Task<List<JournalDetailActualGridDTO>> GetAllJournalActualDetailListAsync()
        {
            R_Exception loEx = new R_Exception();
            List<JournalDetailActualGridDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<JournalDetailActualGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetAllJournalDetailList),
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
        public async Task<CurrencyRateResult> RefreshCurrencyRateAsync()
        {
            var loEx = new R_Exception();
            CurrencyRateResult loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<CurrencyRateResult>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.RefreshCurrencyRate),
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
        public async Task JournalCommitApprovalAsync()
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                await R_HTTPClientWrapper.R_APIRequestObject<JournalCommitApprovalRESULT>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.JournalCommitApproval),
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
        #endregion real function

        #region for implement only
        public InitResult GetInitData()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<StatusDTO> GetStatusList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<CurrencyDTO> GetCurrencyList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<JournalGridDTO> GetAllRecurringList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<JournalGridDTO> GetFilteredRecurringList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<JournalDetailGridDTO> GetAllJournalDetailList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<JournalDetailActualGridDTO> GetAllActualJournalDetailList()
        {
            throw new NotImplementedException();
        }
        public CurrencyRateResult RefreshCurrencyRate()
        {
            throw new NotImplementedException();
        }
        public JournalCommitApprovalRESULT JournalCommitApproval()
        {
            throw new NotImplementedException();
        }
        #endregion for implement only

    }
}
