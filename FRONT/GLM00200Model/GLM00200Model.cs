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
        }
        public async Task<List<JournalDetailActualGridDTO>> GetAllJournalActualDetailListAsync()
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
        }
        public async Task<List<JournalGridDTO>> GetFilteredRecurringListAsync()
        {
            var loEx = new R_Exception();
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
        public async Task<VAR_GL_SYSTEM_PARAM_DTO> GetVAR_GL_SYSTEM_PARAMAsync()
        {
            var loEx = new R_Exception();
            VAR_GL_SYSTEM_PARAM_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GL_SYSTEM_PARAM_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetVAR_GL_SYSTEM_PARAM),
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
        public async Task<VAR_CCURRENT_PERIOD_START_DATE_DTO> GetCCURRENT_PERIOD_START_DATEAsync()
        {
            var loEx = new R_Exception();
            VAR_CCURRENT_PERIOD_START_DATE_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_CCURRENT_PERIOD_START_DATE_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetCCURRENT_PERIOD_START_DATE),
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
        public async Task<VAR_CSOFT_PERIOD_START_DATE_DTO> GetCSOFT_PERIOD_START_DATEAsync()
        {
            var loEx = new R_Exception();
            VAR_CSOFT_PERIOD_START_DATE_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_CSOFT_PERIOD_START_DATE_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetCSOFT_PERIOD_START_DATE),
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
        public async Task<VAR_IUNDO_COMMIT_JRN_DTO> GetIUNDO_COMMIT_JRNAsync()
        {
            var loEx = new R_Exception();
            VAR_IUNDO_COMMIT_JRN_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_IUNDO_COMMIT_JRN_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetIUNDO_COMMIT_JRN),
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
        public async Task<VAR_GSM_TRANSACTION_CODE_DTO> GetGSM_TRANSACTION_CODEAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_TRANSACTION_CODE_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_TRANSACTION_CODE_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetGSM_TRANSACTION_CODE),
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
        public async Task<VAR_GSM_PERIOD_DTO> GetGSM_PERIODAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_PERIOD_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_PERIOD_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetGSM_PERIOD),
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
        public async Task<List<VAR_STATUS_DTO>> GetSTATUS_DTOAsync()
        {
            var loEx = new R_Exception();
            List<VAR_STATUS_DTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<VAR_STATUS_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetSTATUS_DTO),
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
        public async Task<VAR_GSM_COMPANY_DTO> GetVAR_GSM_COMPANY_DTOAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_COMPANY_DTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_COMPANY_DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetVAR_GSM_COMPANY),
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
        public async Task<List<VAR_CURRENCY>> GetVAR_CURRENCIESAsync()
        {
            var loEx = new R_Exception();
            List<VAR_CURRENCY> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<VAR_CURRENCY>(
                    _RequestServiceEndPoint,
                    nameof(IGLM00200.GetVAR_CURRENCIES),
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
        public async Task<REFRESH_CURRENCY_RATE_RESULT> RefreshCurrencyRateAsync()
        {
            var loEx = new R_Exception();
            REFRESH_CURRENCY_RATE_RESULT loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<REFRESH_CURRENCY_RATE_RESULT>(
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
        public IAsyncEnumerable<VAR_STATUS_DTO> GetSTATUS_DTO()
        {
            throw new NotImplementedException();
        }
        public VAR_GL_SYSTEM_PARAM_DTO GetVAR_GL_SYSTEM_PARAM()
        {
            throw new NotImplementedException();
        }
        public VAR_CCURRENT_PERIOD_START_DATE_DTO GetCCURRENT_PERIOD_START_DATE()
        {
            throw new NotImplementedException();
        }
        public VAR_CSOFT_PERIOD_START_DATE_DTO GetCSOFT_PERIOD_START_DATE()
        {
            throw new NotImplementedException();
        }
        public VAR_IUNDO_COMMIT_JRN_DTO GetIUNDO_COMMIT_JRN()
        {
            throw new NotImplementedException();
        }
        public VAR_GSM_TRANSACTION_CODE_DTO GetGSM_TRANSACTION_CODE()
        {
            throw new NotImplementedException();
        }
        public VAR_GSM_PERIOD_DTO GetGSM_PERIOD()
        {
            throw new NotImplementedException();
        }
        public VAR_GSM_COMPANY_DTO GetVAR_GSM_COMPANY()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<VAR_CURRENCY> GetVAR_CURRENCIES()
        {
            throw new NotImplementedException();
        }

        public REFRESH_CURRENCY_RATE_RESULT RefreshCurrencyRate()
        {
            throw new NotImplementedException();
        }

        public JournalCommitApprovalRESULT JournalCommitApproval()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<JournalDetailActualGridDTO> GetAllActualJournalDetailList()
        {
            throw new NotImplementedException();
        }
        #endregion for implement only

    }
}
