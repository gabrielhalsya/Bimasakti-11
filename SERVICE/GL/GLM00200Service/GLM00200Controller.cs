using GLM00200Back;
using GLM00200Common;
using GLM00200Common.DTO_s;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Collections.Generic;
using System.Globalization;

namespace GLM00200Service
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GLM00200Controller : ControllerBase, IGLM00200
    {
        [HttpPost]
        public IAsyncEnumerable<JournalGridDTO> GetAllRecurringList()
        {
            R_Exception loException = new R_Exception();
            List<JournalGridDTO> loRtnTemp = null;
            RecurringJournalListParamDTO loDbParam;
            GLM00200Cls loCls;
            try
            {
                var SearchParam = R_Utility.R_GetContext<RecurringJournalListParamDTO>(RecurringJournalContext.OSEARCH_PARAM);
                loCls = new GLM00200Cls();
                SearchParam.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                SearchParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                SearchParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                SearchParam.CPERIOD_YYYYMM = SearchParam.CPERIOD_YYYY + SearchParam.CPERIOD_MM;
                loRtnTemp = loCls.GetJournalList(SearchParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return JournalListStreamListHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<JournalGridDTO> JournalListStreamListHelper(List<JournalGridDTO> loRtnTemp)
        {
            foreach (JournalGridDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public IAsyncEnumerable<JournalDetailGridDTO> GetAllJournalDetailList()
        {
            R_Exception loException = new R_Exception();
            List<JournalDetailGridDTO> loRtnTemp = null;
            RecurringJournalListParamDTO loDbParam;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetJournalDetailList(new RecurringJournalListParamDTO()
                {
                    CLANGUAGE_ID = R_BackGlobalVar.CULTURE,
                    CREC_ID = R_Utility.R_GetContext<string>(RecurringJournalContext.CREC_ID)
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return JournalDetailListStreamListHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<JournalDetailGridDTO> JournalDetailListStreamListHelper(List<JournalDetailGridDTO> loRtnTemp)
        {
            foreach (JournalDetailGridDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<JournalParamDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceGetRecordResultDTO<JournalParamDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<JournalParamDTO> poParameter)
        {
            R_ServiceGetRecordResultDTO<JournalParamDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new R_ServiceGetRecordResultDTO<JournalParamDTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<JournalParamDTO> R_ServiceSave(R_ServiceSaveParameterDTO<JournalParamDTO> poParameter)
        {
            R_ServiceSaveResultDTO<JournalParamDTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtn = new R_ServiceSaveResultDTO<JournalParamDTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);//call clsMethod to save
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<JournalGridDTO> GetFilteredRecurringList()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public INIT_VAR_RESULT_DTO InitialData()
        {
            INIT_VAR_RESULT_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new INIT_VAR_RESULT_DTO();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        #region init
        [HttpPost]
        public VAR_GL_SYSTEM_PARAM_DTO GetVAR_GL_SYSTEM_PARAM()
        {
            VAR_GL_SYSTEM_PARAM_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_GL_SYSTEM_PARAM_DTO();
                loRtn = loCls.GetVAR_GL_SYSTEM_PARAM(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public VAR_CCURRENT_PERIOD_START_DATE_DTO GetCCURRENT_PERIOD_START_DATE()
        {
            VAR_CCURRENT_PERIOD_START_DATE_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_CCURRENT_PERIOD_START_DATE_DTO();
                loRtn = loCls.GetCCURRENT_PERIOD_START_DATE(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                        CCURRENT_PERIOD_MM = R_Utility.R_GetContext<string>(RecurringJournalContext.CCURRENT_PERIOD_MM),
                        CCURRENT_PERIOD_YY = R_Utility.R_GetContext<string>(RecurringJournalContext.CCURRENT_PERIOD_YY),
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public VAR_CSOFT_PERIOD_START_DATE_DTO GetCSOFT_PERIOD_START_DATE()
        {
            VAR_CSOFT_PERIOD_START_DATE_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_CSOFT_PERIOD_START_DATE_DTO();
                loRtn = loCls.GetCSOFT_PERIOD_START_DATE(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                        CSOFT_PERIOD_YY = R_Utility.R_GetContext<string>(RecurringJournalContext.CSOFT_PERIOD_YY),
                        CSOFT_PERIOD_MM = R_Utility.R_GetContext<string>(RecurringJournalContext.CSOFT_PERIOD_MM),
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public VAR_IUNDO_COMMIT_JRN_DTO GetIUNDO_COMMIT_JRN()
        {
            VAR_IUNDO_COMMIT_JRN_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_IUNDO_COMMIT_JRN_DTO();
                loRtn = loCls.GetIUNDO_COMMIT_JRN(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public VAR_GSM_TRANSACTION_CODE_DTO GetGSM_TRANSACTION_CODE()
        {
            VAR_GSM_TRANSACTION_CODE_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_GSM_TRANSACTION_CODE_DTO();
                loRtn = loCls.GetGSM_TRANSACTION_CODE(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public VAR_GSM_PERIOD_DTO GetGSM_PERIOD()
        {
            VAR_GSM_PERIOD_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_GSM_PERIOD_DTO();
                loRtn = loCls.GetGSM_PERIOD(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<VAR_STATUS_DTO> GetSTATUS_DTO()
        {
            R_Exception loException = new R_Exception();
            List<VAR_STATUS_DTO> loRtnTemp = null;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetSTATUS_DTO(new INIT_VAR_DB_PARAM()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CLANGUAGE_ID = R_BackGlobalVar.CULTURE,
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return StreamVAR_STATUS_DTOHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<VAR_STATUS_DTO> StreamVAR_STATUS_DTOHelper(List<VAR_STATUS_DTO> loRtnTemp)
        {
            foreach (VAR_STATUS_DTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public VAR_GSM_COMPANY_DTO GetVAR_GSM_COMPANY()
        {
            VAR_GSM_COMPANY_DTO loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new VAR_GSM_COMPANY_DTO();
                loRtn = loCls.GetVAR_GSM_COMPANY(
                    new INIT_VAR_DB_PARAM()
                    {
                        CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID
                    });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<VAR_CURRENCY> GetVAR_CURRENCIES()
        {
            R_Exception loException = new R_Exception();
            List<VAR_CURRENCY> loRtnTemp = null;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetCurrency(new INIT_VAR_DB_PARAM()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return StreamVAR_CURRENCY_DTOHelper(loRtnTemp);
        }
        #endregion
        private async IAsyncEnumerable<VAR_CURRENCY> StreamVAR_CURRENCY_DTOHelper(List<VAR_CURRENCY> loRtnTemp)
        {
            foreach (VAR_CURRENCY loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public REFRESH_CURRENCY_RATE_RESULT RefreshCurrencyRate()
        {
            REFRESH_CURRENCY_RATE_RESULT loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            REFRESH_CURRENCY_RATE_PARAM poParam = null;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new REFRESH_CURRENCY_RATE_RESULT();
                poParam = new REFRESH_CURRENCY_RATE_PARAM()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CCURRENCY_CODE= R_Utility.R_GetContext<string>(RecurringJournalContext.CCURRENCY_CODE),
                    CRATETYPE_CODE = R_Utility.R_GetContext<string>(RecurringJournalContext.CRATETYPE_CODE),
                    CSTART_DATE= R_Utility.R_GetContext<string>(RecurringJournalContext.CSTART_DATE),
                };
                loRtn= loCls.RefreshCurrencyRate(poParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }
    }
}