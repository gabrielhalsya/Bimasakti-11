using LMM06500BACK;
using LMM06500COMMON;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using System.Reflection;

namespace LMM06500SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LMM06501Controller : ControllerBase, ILMM06501
    {
        [HttpPost]
        public LMM06500UploadFileDTO DownloadTemplateFile()
        {
            var loEx = new R_Exception();
            var loRtn = new LMM06500UploadFileDTO();

            try
            {
                Assembly loAsm = Assembly.Load("BIMASAKTI_LM_API");

                var lcResourceFile = "BIMASAKTI_LM_API.Template.Staff.xlsx";
                using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                {
                    var ms = new MemoryStream();
                    resFilestream.CopyTo(ms);
                    var bytes = ms.ToArray();

                    loRtn.FileBytes = bytes;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }


        [HttpPost]
        public IAsyncEnumerable<LMM06500DTO> GetStaffUploadList()
        {
            var loEx = new R_Exception();
            IAsyncEnumerable<LMM06500DTO> loRtn = null;
            var loParameter = new LMM06500DTO();

            try
            {
                var loCls = new LMM06501Cls();

                loParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParameter.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);

                var loResult = loCls.GetAllStaffUpload(loParameter);

                loRtn = GetStream<LMM06500DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<LMM06501ErrorValidateDTO> GetErrorProcess()
        {
            var loEx = new R_Exception();
            IAsyncEnumerable<LMM06501ErrorValidateDTO> loRtn = null;

            try
            {
                var lcKeyGuid = R_Utility.R_GetStreamingContext<string>("UploadStaffKeyGuid");

                var loCls = new LMM06500UploadStaffCls();

                var loResult = loCls.GetErrorProcess(R_BackGlobalVar.COMPANY_ID, R_BackGlobalVar.USER_ID, lcKeyGuid);

                loRtn = GetStream<LMM06501ErrorValidateDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        private async IAsyncEnumerable<T> GetStream<T>(List<T> poList)
        {
            foreach (var item in poList)
            {
                yield return item;
            }
        }
    }
}
