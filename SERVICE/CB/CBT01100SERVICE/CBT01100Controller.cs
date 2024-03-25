using CBT01100BACK;
using CBT01100COMMON;
using CBT01100COMMON.DTO_s;
using CBT01100COMMON.Loggers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CBT01100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CBT01100Controller : ControllerBase, ICBT01100
    {
        private LoggerCBT01100 _logger;

        private readonly ActivitySource _activitySource;

        public CBT01100Controller(ILogger<LoggerCBT01100> logger)
        {
            //Initial and Get Logger
            LoggerCBT01100.R_InitializeLogger(logger);
            _logger = LoggerCBT01100.R_GetInstanceLogger();
            _activitySource = CBT01100Activity.R_InitializeAndGetActivitySource(GetType().Name);

        }

        [HttpPost]
        public IAsyncEnumerable<CBT01110DTO> GetJournalDetailList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<CBT01101DTO> loRtn = null;

            try
            {
                var loRecId = R_Utility.R_GetStreamingContext<string>(ContextConstantCBT01100.CREC_ID);
                var loCls = new CBT01100Cls();
                var loTempRtn = loCls.GetJournalDetailList(loRecId);
                ShowLogExecute();
                loRtn = StreamListData<CBT01101DTO>(loTempRtn);
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
        public IAsyncEnumerable<CBT01100DTO> GetJournalList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<CBT01100DTO> loRtn = null;

            try
            {
                var loParam = new CBT01100ParamDTO();
                loParam.CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstantCBT01100.CDEPT_CODE);
                loParam.CPERIOD = R_Utility.R_GetStreamingContext<string>(ContextConstantCBT01100.CPERIOD);
                loParam.CSTATUS = R_Utility.R_GetStreamingContext<string>(ContextConstantCBT01100.CSTATUS);
                loParam.CSEARCH_TEXT = R_Utility.R_GetStreamingContext<string>(ContextConstantCBT01100.CSEARCH_TEXT);

                var loCls = new CBT01100Cls();

                ShowLogExecute();
                var loTempRtn = loCls.GetJournalList(loParam);

                loRtn = StreamListData<CBT01100DTO>(loTempRtn);
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
        public CBT01100RecordResult<CBT01100UpdateStatusDTO> UpdateJournalStatus(CBT01100UpdateStatusDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01100RecordResult<CBT01100UpdateStatusDTO> loRtn = new CBT01100RecordResult<CBT01100UpdateStatusDTO>();

            try
            {
                var loCls = new CBT01100Cls();

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
        public CBT01100RecordResult<CBT01100RapidApprovalValidationDTO> ValidationRapidApproval(CBT01100RapidApprovalValidationDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01100RecordResult<CBT01100RapidApprovalValidationDTO> loRtn = new CBT01100RecordResult<CBT01100RapidApprovalValidationDTO>();

            try
            {
                var loCls = new CBT01100Cls();

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
        private async IAsyncEnumerable<T> StreamListData<T>(List<T> poParameter)
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
