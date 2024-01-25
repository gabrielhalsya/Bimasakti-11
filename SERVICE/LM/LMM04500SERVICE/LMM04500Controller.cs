﻿using LMM04500BACK;
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
                loRtnTemp = loCls.GetPropertyList(new PropertyDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
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
                loRtnTemp = loCls.GetUnitTypeCategoryList(new UnitTypeCategoryParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstantLMM04500.CPROPERTY_ID)
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

        public IAsyncEnumerable<PricingDTO> GetPricingDateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingDateList(new PricingParamDTO()
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

        public IAsyncEnumerable<PricingDTO> GetPricingList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingDTO> loRtnTemp = null;
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingList(new PricingParamDTO()
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

        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poList)
        {
            foreach (T loEntity in poList)
            {
                yield return loEntity;
            }
        }

        public PricingDumpResultDTO SavePricing(PricingSaveParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            PricingDumpResultDTO loRtn = new();
            R_Exception loException = new R_Exception();
            LMM04500Cls loCls;
            try
            {
                loCls = new LMM04500Cls();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                ShowLogExecute();
                loCls.SavePricing(poParam);
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

        #region (CRUD)NotImplementedException

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<PricingSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<PricingSaveParamDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<PricingSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();

        }

        public R_ServiceSaveResultDTO<PricingSaveParamDTO> R_ServiceSave(R_ServiceSaveParameterDTO<PricingSaveParamDTO> poParameter)
        {
            throw new NotImplementedException();

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
