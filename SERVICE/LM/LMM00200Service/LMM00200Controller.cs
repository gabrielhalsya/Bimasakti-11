using LMM00200Back;
using LMM00200Common;
using LMM00200Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using R_OpenTelemetry;

namespace LMM00200Service
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LMM00200Controller : ControllerBase, ILMM00200
    {
        private LMM00200Logger _logger;

        private readonly ActivitySource _activitySource;

        public LMM00200Controller(ILogger<LMM00200Controller> logger)
        {
            //initiate
            LMM00200Logger.R_InitializeLogger(logger);
            _logger = LMM00200Logger.R_GetInstanceLogger();
            _activitySource = LMM00200Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        [HttpPost]
        public IAsyncEnumerable<LMM00200GridDTO> GetUserParamList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<LMM00200GridDTO> loRtnTemp = null;
            LMM00200DBParam loDbParam;
            LMM00200Cls loCls;
            try
            {
                loCls = new LMM00200Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetUserParamList(new LMM00200DBParam()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID
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
            return LMM00200StreamListHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<T> LMM00200StreamListHelper<T>(List<T> loRtnTemp)
        {
            foreach (T loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<LMM00200DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceGetRecordResultDTO<LMM00200DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<LMM00200DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceGetRecordResultDTO<LMM00200DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM00200Cls loCls;
            try
            {
                loCls = new LMM00200Cls(); //create cls class instance
                loRtn = new R_ServiceGetRecordResultDTO<LMM00200DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
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

        [HttpPost]
        public R_ServiceSaveResultDTO<LMM00200DTO> R_ServiceSave(R_ServiceSaveParameterDTO<LMM00200DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_ServiceSaveResultDTO<LMM00200DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            LMM00200Cls loCls;
            try
            {
                loCls = new LMM00200Cls();
                loRtn = new R_ServiceSaveResultDTO<LMM00200DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                ShowLogExecute();
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);//call clsMethod to save
            }
            catch (Exception ex)
            {
                loException.Add(ex); ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public LMM00200ActiveInactiveParamDTO GetActiveParam(ActiveInactiveParam poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            LMM00200ActiveInactiveParamDTO loRtn = null;
            R_Exception loException = new R_Exception();
            LMM00200Cls loCls;
            try
            {
                loCls = new LMM00200Cls();
                loRtn = new LMM00200ActiveInactiveParamDTO();
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                //call clsMethod to save
                ShowLogExecute();
                loCls.ActiveInactiveUserParam(poParam);
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

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);
    }
}