using GLM00200BACK;
using GLM00200COMMON;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GLM00200SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLM00200Controller : ControllerBase, IGLM00200
    {
        private LoggerGLM00200 _logger;

        private readonly ActivitySource _activitySource;

        public GLM00200Controller(ILogger<LoggerGLM00200> logger)
        {
            //Initial and Get Logger
            LoggerGLM00200.R_InitializeLogger(logger);
            _logger = LoggerGLM00200.R_GetInstanceLogger();
            _activitySource = GLM00200Activity.R_InitializeAndGetActivitySource(GetType().Name);

        }

        [HttpPost]
        public IAsyncEnumerable<GLM00201DTO> GetJournalDetailList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<GLM00201DTO> loRtn = null;

            try
            {
                var loRecId = R_Utility.R_GetStreamingContext<string>(ContextConstant.CREC_ID);
                var loCls = new GLM00200Cls();
                var loTempRtn = loCls.GetJournalDetailList(loRecId);
                ShowLogExecute();
                loRtn = StreamListHelper<GLM00201DTO>(loTempRtn);
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
        public IAsyncEnumerable<GLM00200DTO> GetJournalList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<GLM00200DTO> loRtn = null;

            try
            {
                var loParam = new GLM00200ParamDTO();
                loParam.CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CDEPT_CODE);
                loParam.CPERIOD = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPERIOD);
                loParam.CSTATUS = R_Utility.R_GetStreamingContext<string>(ContextConstant.CSTATUS);
                loParam.CSEARCH_TEXT = R_Utility.R_GetStreamingContext<string>(ContextConstant.CSEARCH_TEXT);

                var loCls = new GLM00200Cls();

                ShowLogExecute();
                var loTempRtn = loCls.GetJournalList(loParam);

                loRtn = StreamListHelper<GLM00200DTO>(loTempRtn);
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
        public GLM00200RecordResult<GLM00200UpdateStatusDTO> UpdateJournalStatus(GLM00200UpdateStatusDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200UpdateStatusDTO> loRtn = new GLM00200RecordResult<GLM00200UpdateStatusDTO>();

            try
            {
                var loCls = new GLM00200Cls();

                ShowLogExecute();
                loCls.UpdateJournalStatus(poEntity);
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
        public GLM00200RecordResult<GLM00200RapidApprovalValidationDTO> ValidationRapidApproval(GLM00200RapidApprovalValidationDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            GLM00200RecordResult<GLM00200RapidApprovalValidationDTO> loRtn = new GLM00200RecordResult<GLM00200RapidApprovalValidationDTO>();

            try
            {
                var loCls = new GLM00200Cls();

                ShowLogExecute();
                loRtn.Data = loCls.ValidationRapidAppro(poEntity);
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

        #region Stream List Data
        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }
        #endregion

        #region logger

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);

        #endregion
    }


}