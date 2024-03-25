using CBT01100BACK;
using CBT01100COMMON;
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
    public class CBT01110Controller : ControllerBase, ICBT01110
    {
        private LoggerCBT01100 _logger;

        private readonly ActivitySource _activitySource;

        public CBT01110Controller(ILogger<LoggerCBT01100> logger)
        {
            LoggerCBT01100.R_InitializeLogger(logger);
            _logger = LoggerCBT01100.R_GetInstanceLogger();
            _activitySource = CBT01100Activity.R_InitializeAndGetActivitySource(GetType().Name);
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


        [HttpPost]
        public CBT01100RecordResult<CBT01110LastCurrencyRateDTO> GetLastCurrency(CBT01110LastCurrencyRateDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01100RecordResult<CBT01110LastCurrencyRateDTO> loRtn = new CBT01100RecordResult<CBT01110LastCurrencyRateDTO>();

            try
            {
                var loCls = new CBT01110Cls();
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
        public CBT01100RecordResult<CBT01110DTO> GetJournalRecord(CBT01110DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01100RecordResult<CBT01110DTO> loRtn = new CBT01100RecordResult<CBT01110DTO>();

            try
            {
                var loCls = new CBT01110Cls();

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
        public CBT01100RecordResult<CBT01110DTO> SaveJournal(CBT01110HeaderDetailDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01100RecordResult<CBT01110DTO> loRtn = new CBT01100RecordResult<CBT01110DTO>();

            try
            {
                var loCls = new CBT01110Cls();

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
