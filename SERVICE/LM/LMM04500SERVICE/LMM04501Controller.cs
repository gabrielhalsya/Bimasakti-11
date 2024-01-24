using LMM04500BACK;
using LMM04500COMMON;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500SERVICE
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LMM04501Controller : ControllerBase, ILMM04501
    {
        private LoggerLMM04500 _logger;

        private readonly ActivitySource _activitySource;

        public LMM04501Controller(ILogger<LoggerLMM04500> logger)
        {
            LoggerLMM04500.R_InitializeLogger(logger);
            _logger = LoggerLMM04500.R_GetInstanceLogger();
            _activitySource = LMM04500Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        public IAsyncEnumerable<PricingRateDTO> GetPricingRateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingRateDTO> loRtnTemp = null;
            LMM04501Cls loCls;
            try
            {
                loCls = new LMM04501Cls();
                ShowLogExecute();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return StreamListHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poList)
        {
            foreach (T loEntity in poList)
            {
                yield return loEntity;
            }
        }

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<PricingRateDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<PricingRateDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<PricingRateDTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceGetRecordResultDTO<PricingRateDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM04501Cls loCls;
            try
            {
                loCls = new LMM04501Cls(); //create cls class instance
                loRtn = new R_ServiceGetRecordResultDTO<PricingRateDTO>();
                ShowLogExecute();
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        public R_ServiceSaveResultDTO<PricingRateDTO> R_ServiceSave(R_ServiceSaveParameterDTO<PricingRateDTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceSaveResultDTO<PricingRateDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM04501Cls loCls;
            try
            {
                loCls = new LMM04501Cls();
                loRtn = new R_ServiceSaveResultDTO<PricingRateDTO>();
                ShowLogExecute();
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        

        #region logger
        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);

        public void SavePricing(PricingParamDTO poParam)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
