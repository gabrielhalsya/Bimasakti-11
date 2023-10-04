using GSM04000Common;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM04000Back;

namespace GSM04000Service
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GSM04100Controller : ControllerBase, IGSM04100
    {
        [HttpPost]
        public GSM04100ListDTO GetGSM04100ListByDeptCode()
        {
            GSM04100ListDTO loRtn = null; //declare IAsyncEnumerable<> for return
            R_Exception loException = new R_Exception(); //declare exeption instance for trycatch
            GSM04100ListDBParameterDTO loDbParam; //dec
            GSM04100Cls loCls;
            try
            {
                loRtn = new GSM04100ListDTO();
                loDbParam = new GSM04100ListDBParameterDTO();
                loDbParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbParam.CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CDEPT_CODE);
                loCls = new GSM04100Cls();
                loRtn.Data = loCls.GetUserDeptList(loDbParam); ;
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
        public IAsyncEnumerable<GSM04100DTO> GetUserToAssignList()
        {
            List<GSM04100DTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM04100Cls loCls;
            try
            {
                loCls = new GSM04100Cls();
                loRtnTemp = loCls.GetUserToAssignList(new GSM04100ListDBParameterDTO()
                {
                    CPROGRAM_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROGRAM_CODE),
                    CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CDEPT_CODE),
                    CCOMPANY_ID=R_BackGlobalVar.COMPANY_ID,
                }
                );
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return GetUserListHelper(loRtnTemp);

        }
        private async IAsyncEnumerable<GSM04100DTO> GetUserListHelper(List<GSM04100DTO> loRtnTemp)
        {
            foreach (GSM04100DTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM04100DTO> poParameter)
        {
            R_ServiceDeleteResultDTO loRtn = null;
            R_Exception loException = new R_Exception();
            GSM04100Cls loCls;
            try
            {
                loRtn = new R_ServiceDeleteResultDTO();
                loCls = new GSM04100Cls(); //create cls class instance
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_LOGIN_ID = R_BackGlobalVar.USER_ID;
                loCls.R_Delete(poParameter.Entity);
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
        public R_ServiceGetRecordResultDTO<GSM04100DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM04100DTO> poParameter)
        {
            R_ServiceGetRecordResultDTO<GSM04100DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GSM04100Cls loCls;
            try
            {
                loCls = new GSM04100Cls(); //create cls class instance
                loRtn = new R_ServiceGetRecordResultDTO<GSM04100DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CDEPT_CODE);
                poParameter.Entity.CUSER_LOGIN_ID = R_BackGlobalVar.USER_ID;
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
        public R_ServiceSaveResultDTO<GSM04100DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM04100DTO> poParameter)
        {
            R_ServiceSaveResultDTO<GSM04100DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GSM04100Cls loCls;
            try
            {
                loRtn = new R_ServiceSaveResultDTO<GSM04100DTO>();
                loCls = new GSM04100Cls(); //create instance
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_LOGIN_ID = R_BackGlobalVar.USER_ID;
                loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
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