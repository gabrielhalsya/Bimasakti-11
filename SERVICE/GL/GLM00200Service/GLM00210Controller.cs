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
    public class GLM00210Controller : ControllerBase, IGLM00210
    {
        private LoggerGLM00200 _logger;

        private readonly ActivitySource _activitySource;

        public GLM00210Controller(ILogger<LoggerGLM00200> logger)
        {
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
        public GLM00200RecordResult<GLM00210LastCurrencyRateDTO> GetLastCurrency(GLM00210LastCurrencyRateDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00210LastCurrencyRateDTO> loRtn = new GLM00200RecordResult<GLM00210LastCurrencyRateDTO>();

            try
            {
                var loCls = new GLM00210Cls();
                ShowLogExecute();
                loRtn.Data = loCls.GetLastCurrency(poEntity);
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
        public GLM00200RecordResult<GLM00210DTO> GetJournalRecord(GLM00210DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00210DTO> loRtn = new GLM00200RecordResult<GLM00210DTO>();

            try
            {
                var loCls = new GLM00210Cls();

                loRtn.Data = loCls.GetJournalDisplay(poEntity);
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
        public GLM00200RecordResult<GLM00210DTO> SaveJournal(GLM00210HeaderDetailDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00210DTO> loRtn = new GLM00200RecordResult<GLM00210DTO>();

            try
            {
                var loCls = new GLM00210Cls();

                //Save
                var loResult = loCls.SaveJournal(poEntity);
                //Get
                loRtn.Data = loCls.GetJournalDisplay(loResult);
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