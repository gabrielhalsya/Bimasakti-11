using LMM04500BACK;
using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_BackEnd;
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

        public IAsyncEnumerable<PricingRateDTO> GetPricingRateDateList()
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
                loRtnTemp = loCls.GetPricingRateDateList(new PricingParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CPROPERTY_ID),
                    CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CPRICE_TYPE),
                    CTYPE = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CTYPE),
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                });
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
                loRtnTemp = loCls.GetPricingRateList(new PricingParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CPROPERTY_ID),
                    CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CPRICE_TYPE),
                    LACTIVE = R_Utility.R_GetStreamingContext<bool>(ContextConstantLMM04500.LACTIVE),
                    CTYPE = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CTYPE),
                    CVALID_DATE = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CVALID_DATE),
                    CVALID_INTERNAL_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CVALID_ID),
                });
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

        public PricingDumpResultDTO SavePricingRate(PricingRateSaveParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            PricingDumpResultDTO loRtn = new();
            R_Exception loException = new R_Exception();
            LMM04501Cls loCls;
            try
            {
                loCls = new LMM04501Cls();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                ShowLogExecute();
                loCls.SavePricingRate(poParam);
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

        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poList)
        {
            foreach (T loEntity in poList)
            {
                yield return loEntity;
            }
        }

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<PricingRateSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<PricingRateSaveParamDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<PricingRateSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceSaveResultDTO<PricingRateSaveParamDTO> R_ServiceSave(R_ServiceSaveParameterDTO<PricingRateSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        #region logger
        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);

        

        #endregion
    }
}
