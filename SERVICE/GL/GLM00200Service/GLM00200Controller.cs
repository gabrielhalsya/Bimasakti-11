using GLM00200Back;
using GLM00200Common;
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
        public IAsyncEnumerable<JournalDetailActualGridDTO> GetAllActualJournalDetailList()
        {
            R_Exception loException = new R_Exception();
            List<JournalDetailActualGridDTO> loRtnTemp = null;
            RecurringJournalListParamDTO loDbParam;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetActualJournalDetailList(new RecurringJournalListParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CDEPT_CODE = R_Utility.R_GetContext<string>(RecurringJournalContext.CDEPT_CODE),
                    CREF_NO = R_Utility.R_GetContext<string>(RecurringJournalContext.CREF_NO),
                    CLANGUAGE_ID = R_BackGlobalVar.CULTURE,
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return ActualJournalDetailListStreamListHelper(loRtnTemp);
        }

        private async IAsyncEnumerable<JournalDetailActualGridDTO> ActualJournalDetailListStreamListHelper(List<JournalDetailActualGridDTO> loRtnTemp)
        {
            foreach (JournalDetailActualGridDTO loEntity in loRtnTemp)
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
        public CurrencyRateResult RefreshCurrencyRate()
        {
            CurrencyRateResult loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            CurrencyRateParamDTO poParam = null;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new CurrencyRateResult();
                poParam = new CurrencyRateParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CCURRENCY_CODE = R_Utility.R_GetContext<string>(RecurringJournalContext.CCURRENCY_CODE),
                    CRATETYPE_CODE = R_Utility.R_GetContext<string>(RecurringJournalContext.CRATETYPE_CODE),
                    CSTART_DATE = R_Utility.R_GetContext<string>(RecurringJournalContext.CSTART_DATE),
                };
                loRtn = loCls.RefreshCurrencyRate(poParam);
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
        public JournalCommitApprovalRESULT JournalCommitApproval()
        {
            JournalCommitApprovalRESULT loRtn = null;
            R_Exception loException = new R_Exception();
            GLM00200Cls loCls;
            JournalCommitApprovalPARAM poParam = null;
            try
            {
                loCls = new GLM00200Cls(); //create cls class instance
                loRtn = new JournalCommitApprovalRESULT();
                poParam = new JournalCommitApprovalPARAM()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CAPPROVE_BY = R_BackGlobalVar.USER_ID,
                    CJRN_ID_LIST = R_Utility.R_GetContext<string>(RecurringJournalContext.CJRN_ID_LIST),
                    CNEW_STATUS = R_Utility.R_GetContext<string>(RecurringJournalContext.CNEW_STATUS),
                    LAUTO_COMMIT = R_Utility.R_GetContext<bool>(RecurringJournalContext.LAUTO_COMMIT),
                    LUNDO_COMMIT = R_Utility.R_GetContext<bool>(RecurringJournalContext.LAUTO_COMMIT),
                    EMODE = R_Utility.R_GetContext<EModeCmmtAprvJRN>(RecurringJournalContext.EMODE)
                };
                loCls.CommitApproveJournal(poParam);
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
        public InitResult GetInitData()
        {
            R_Exception loException = new R_Exception();
            InitResult loRtn = null;
            RecurringJournalListParamDTO loDbParam;
            GLM00200Cls loCls;
            try
            {
                var SearchParam = R_Utility.R_GetContext<RecurringJournalListParamDTO>(RecurringJournalContext.OSEARCH_PARAM);
                loCls = new GLM00200Cls();
                loRtn.data = loCls.GetInitData(new InitParamDTO()
                {
                    CLANGUAGE_ID = R_BackGlobalVar.CULTURE,
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID
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
        public IAsyncEnumerable<StatusDTO> GetStatusList()
        {
            R_Exception loException = new R_Exception();
            List<StatusDTO> loRtnTemp = null;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetSTATUS_DTO(new InitParamDTO()
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

        private async IAsyncEnumerable<StatusDTO> StreamVAR_STATUS_DTOHelper(List<StatusDTO> loRtnTemp)
        {
            foreach (StatusDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public IAsyncEnumerable<CurrencyDTO> GetCurrencyList()
        {
            R_Exception loException = new R_Exception();
            List<CurrencyDTO> loRtnTemp = null;
            GLM00200Cls loCls;
            try
            {
                loCls = new GLM00200Cls();
                loRtnTemp = loCls.GetCurrency(new InitParamDTO()
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
        private async IAsyncEnumerable<CurrencyDTO> StreamVAR_CURRENCY_DTOHelper(List<CurrencyDTO> loRtnTemp)
        {
            foreach (CurrencyDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }


        #endregion


    }
}