using GLT00100Back;
using GLT00100Common;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GLT00100Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLT00100Controller : ControllerBase, IGLT00100
    {
        public IAsyncEnumerable<GLT00100DetailDTO> GetAllJournalDetailList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<GLT00100DetailDTO> loRtn = null;
            GLT00100ParamDTO loDbPar;
            GLT00100Cls loCls;
            List<GLT00100DetailDTO> loRtnTemp;
            try
            {
                loCls = new GLT00100Cls();
                loDbPar = new GLT00100ParamDTO();
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loDbPar.CREC_ID = R_Utility.R_GetContext<string>(GLT00100ContextConstant.CREC_ID);
                loRtnTemp = loCls.GetJournalDetailList(loDbPar);
                loRtn = StreamHelper(loRtnTemp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            return loRtn;
        }

        public IAsyncEnumerable<GLT00100GridDTO> GetJournalList()
        {
            R_Exception loEx = new R_Exception();
            IAsyncEnumerable<GLT00100GridDTO> loRtn = null;
            GLT00100ParamDTO loDbPar;
            GLT00100Cls loCls;
            List<GLT00100GridDTO> loRtnTemp;
            try
            {
                loDbPar = new GLT00100ParamDTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;

                loDbPar.CPERIOD = R_Utility.R_GetContext<string>(GLT00100ContextConstant.CPERIOD);
                loDbPar.CSEARCH_TEXT = R_Utility.R_GetContext<string>(GLT00100ContextConstant.CSEARCH_TEXT);
                loDbPar.CDEPT_CODE = R_Utility.R_GetContext<string>(GLT00100ContextConstant.CDEPT_CODE);
                loDbPar.CSTATUS = R_Utility.R_GetContext<string>(GLT00100ContextConstant.CSTATUS);
                loCls = new GLT00100Cls();
                loRtnTemp = loCls.GetJournalList(loDbPar);
                loRtn = StreamHelper(loRtnTemp);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        private async IAsyncEnumerable<T> StreamHelper<T>(List<T> poList)
        {
            foreach (var item in poList)
            {
                yield return item;
            }
        }

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GLT00100DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<GLT00100DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GLT00100DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceGetRecordResultDTO<GLT00100DTO>();
            GLT00100ParamDTO loDbPar;
            try
            {
                var loCls = new GLT00100Cls();
                loDbPar = new GLT00100ParamDTO();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        public R_ServiceSaveResultDTO<GLT00100DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GLT00100DTO> poParameter)
        {
            R_ServiceSaveResultDTO<GLT00100DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GLT00100Cls loCls;
            try
            {
                loCls = new GLT00100Cls();
                loRtn = new R_ServiceSaveResultDTO<GLT00100DTO>();
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
    }
}