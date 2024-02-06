using GLM00200BACK;
using GLM00200COMMON;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_BackEnd;
using R_Common;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GLM00200SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLM00200InitController : ControllerBase, IGLM00200Init
    {
        private LoggerGLM00200 _logger;
        private readonly ActivitySource _activitySource;

        public GLM00200InitController(ILogger<LoggerGLM00200> logger)
        {
            //Initial and Get Logger
            LoggerGLM00200.R_InitializeLogger(logger);
            _logger = LoggerGLM00200.R_GetInstanceLogger();
            _activitySource = GLM00200Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        #region Stream List Data
        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }
        #endregion

        [HttpPost]
        public IAsyncEnumerable<GLM00200GSCenterDTO> GetCenterList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<GLM00200GSCenterDTO> loRtn = null;

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                var loTempRtn = loCls.GetCenterList();

                loRtn = StreamListHelper<GLM00200GSCenterDTO>(loTempRtn);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            _logger.LogInfo("End GetCenterList");

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GLM00200GSCurrencyDTO> GetCurrencyList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<GLM00200GSCurrencyDTO> loRtn = null;

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                var loTempRtn = loCls.GetCurrencyList();

                loRtn = StreamListHelper<GLM00200GSCurrencyDTO>(loTempRtn);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GLSystemParamDTO> GetGLSystemParam()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GLSystemParamDTO> loRtn = new GLM00200RecordResult<GLM00200GLSystemParamDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetGLSystemParamRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GLM00200GSGSBCodeDTO> GetGSBCodeList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<GLM00200GSGSBCodeDTO> loRtn = null;

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                var loTempRtn = loCls.GetGSBCodeList();

                loRtn = StreamListHelper<GLM00200GSGSBCodeDTO>(loTempRtn);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GSCompanyInfoDTO> GetGSCompanyInfo()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GSCompanyInfoDTO> loRtn = new GLM00200RecordResult<GLM00200GSCompanyInfoDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetCompanyInfoRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GSPeriodDTInfoDTO> GetGSPeriodDTInfo(GLM00200ParamGSPeriodDTInfoDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GSPeriodDTInfoDTO> loRtn = new GLM00200RecordResult<GLM00200GSPeriodDTInfoDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetPeriodDTInfoRecord(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GSPeriodYearRangeDTO> GetGSPeriodYearRange()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GSPeriodYearRangeDTO> loRtn = new GLM00200RecordResult<GLM00200GSPeriodYearRangeDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetPeriodYearRangeRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GLSystemEnableOptionInfoDTO> GetGSSystemEnableOptionInfo()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GLSystemEnableOptionInfoDTO> loRtn = new GLM00200RecordResult<GLM00200GLSystemEnableOptionInfoDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetGLSystemEnableOptionRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200GSTransInfoDTO> GetGSTransCodeInfo()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200GSTransInfoDTO> loRtn = new GLM00200RecordResult<GLM00200GSTransInfoDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetTransCodeInfoRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200InitDTO> GetTabJournalListInitVar()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200InitDTO> loRtn = new GLM00200RecordResult<GLM00200InitDTO>();

            try
            {
                var loCls = new GLM00200InitCls();
                ShowLogExecute();

                loRtn.Data = new()
                {
                    VAR_GL_SYSTEM_PARAM = loCls.GetGLSystemParamRecord(),
                    VAR_GSM_TRANSACTION_CODE = loCls.GetTransCodeInfoRecord(),
                    VAR_IUNDO_COMMIT_JRN = loCls.GetGLSystemEnableOptionRecord(),
                    VAR_GSB_CODE_LIST = loCls.GetGSBCodeList(),
                    VAR_GSM_PERIOD = loCls.GetPeriodYearRangeRecord(),
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00200TodayDateDTO> GetTodayDate()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200TodayDateDTO> loRtn = new GLM00200RecordResult<GLM00200TodayDateDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetTodayDateRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public GLM00200RecordResult<GLM00210InitDTO> GetTabJournalEntryInitVar()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00210InitDTO> loRtn = new GLM00200RecordResult<GLM00210InitDTO>();

            try
            {
                var loCls = new GLM00200InitCls();

                ShowLogExecute();
                loRtn.Data = new()
                {
                    VAR_GSM_TRANSACTION_CODE = loCls.GetTransCodeInfoRecord(),
                    VAR_GSM_COMPANY = loCls.GetCompanyInfoRecord(),
                    VAR_TODAY = loCls.GetTodayDateRecord(),
                    VAR_GL_SYSTEM_PARAM = loCls.GetGLSystemParamRecord(),
                    VAR_CENTER_LIST = loCls.GetCenterList(),
                    VAR_CURRENCY_LIST = loCls.GetCurrencyList(),
                    VAR_GSB_CODE_LIST = loCls.GetGSBCodeList(),
                    VAR_IUNDO_COMMIT_JRN = loCls.GetGLSystemEnableOptionRecord(),
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }


        #region logger

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);


        #endregion
    }
}