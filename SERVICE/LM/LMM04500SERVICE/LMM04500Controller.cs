using LMM04500BACK;
using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LMM04500SERVICE
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LMM04500Controller : ControllerBase, ILMM04500
    {
        private LoggerLMM04500 _logger;

        private readonly ActivitySource _activitySource;

        public LMM04500Controller(ILogger<LMM04500Controller> logger)
        {
            //initiate
            LoggerLMM04500.R_InitializeLogger(logger);
            _logger = LoggerLMM04500.R_GetInstanceLogger();
            _activitySource = LMM04500Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        public IAsyncEnumerable<PricingParamDTO> GetPricingDateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingParamDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
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

        public IAsyncEnumerable<PricingParamDTO> GetPricingList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingParamDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
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

        public IAsyncEnumerable<PropertyDTO> GetPropertyList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PropertyDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
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

        public IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<UnitTypeCategoryDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
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

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<PricingParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<PricingParamDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<PricingParamDTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceGetRecordResultDTO<PricingParamDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls(); //create cls class instance
                loRtn = new R_ServiceGetRecordResultDTO<PricingParamDTO>();
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

        public R_ServiceSaveResultDTO<PricingParamDTO> R_ServiceSave(R_ServiceSaveParameterDTO<PricingParamDTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceSaveResultDTO<PricingParamDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
                loRtn = new R_ServiceSaveResultDTO<PricingParamDTO>();
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
        #endregion
    }
}
