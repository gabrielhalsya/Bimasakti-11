using GLTR00100BACK;
using GLTR00100COMMON;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;

namespace GLTR00100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLTR00100Controller : ControllerBase, IGLTR00100
    {
        [HttpPost]
        public GLTR00100Record<GLTR00100DTO> GetGLJournal(GLTR00100DTO poParam)
        {
            var loEx = new R_Exception();
            GLTR00100Record<GLTR00100DTO> loRtn = null;

            try
            {
                loRtn = new GLTR00100Record<GLTR00100DTO>();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParam.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;

                var loCls = new GLTR00100Cls();

                loRtn.Data = loCls.GetGLJournalTransaction(poParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GLTR00100InitialDTO GetInitialVar()
        {
            var loEx = new R_Exception();
            GLTR00100InitialDTO loRtn = null;

            try
            {
                var poParam = new GLTR00100InitialDTO();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParam.CUSER_LANGUAGE = R_BackGlobalVar.CULTURE;

                var loCls = new GLTR00100Cls();

                loRtn = loCls.GetInitial(poParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}