using GLM00200COMMON;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GLM00200Model
{
    public class GLM00200Model : R_BusinessObjectServiceClientBase<GLM00200ParamDTO>, IGLM00200
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
        public async Task<InitResultDTO> GetInitDataAsync()
        {
            var loEx = new R_Exception();
            InitResultDTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<InitResultDTO>(
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

        public IAsyncEnumerable<GLM00200DTO> GetJournalList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GLM00201DTO> GetJournalDetailList()
        {
            throw new NotImplementedException();
        }

        public GLM00200RecordResult<GLM00200UpdateStatusDTO> UpdateJournalStatus(GLM00200UpdateStatusDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public GLM00200RecordResult<GLM00200RapidApprovalValidationDTO> ValidationRapidApproval(GLM00200RapidApprovalValidationDTO poEntity)
        {
            throw new NotImplementedException();
        }
        #endregion real function

        #region for implement only


        #endregion for implement only

    }
}
