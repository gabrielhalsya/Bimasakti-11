using BaseHeaderReportCommon;
using BaseHeaderReportCommon.BaseHeader;
using BaseHeaderReportCommon.Model;
using GLTR00100BACK;
using GLTR00100COMMON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_ReportFastReportBack;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace GLTR00100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLTR00100PrintController : ControllerBase
    {
        private R_ReportFastReportBackClass _ReportCls;
        private GLTR00100PrintParamDTO _AllGLTR00100Parameter;

        #region instantiate
        public GLTR00100PrintController()
        {
            _ReportCls = new R_ReportFastReportBackClass();
            _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
            _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
            _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
        }
        #endregion

        #region Event Handler
        private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
        {
            pcFileTemplate = "Reports\\GLTR00100.frx";
        }

        private void _ReportCls_R_GetMainDataAndName(ref ArrayList poData, ref string pcDataSourceName)
        {
            poData.Add(GenerateDataPrint(_AllGLTR00100Parameter));
            pcDataSourceName = "ResponseDataModel";
        }

        private void _ReportCls_R_SetNumberAndDateFormat(ref R_ReportFormatDTO poReportFormat)
        {
            poReportFormat.DecimalSeparator = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_SEPARATOR;
            poReportFormat.GroupSeparator = R_BackGlobalVar.REPORT_FORMAT_GROUP_SEPARATOR;
            poReportFormat.DecimalPlaces = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_PLACES;
            poReportFormat.ShortDate = R_BackGlobalVar.REPORT_FORMAT_SHORT_DATE;
            poReportFormat.ShortTime = R_BackGlobalVar.REPORT_FORMAT_SHORT_TIME;
        }
        #endregion

        [HttpPost]
        public R_DownloadFileResultDTO AllJournalTransactionPost(GLTR00100PrintParamDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            R_DownloadFileResultDTO loRtn = null;
            try
            {
                loRtn = new R_DownloadFileResultDTO();
                R_DistributedCache.R_Set(loRtn.GuidResult, R_NetCoreUtility.R_SerializeObjectToByte<GLTR00100PrintParamDTO>(poParameter));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpGet, AllowAnonymous]
        public FileStreamResult AllStreamJournalTransactionsGet(string pcGuid)
        {
            R_Exception loException = new R_Exception();
            FileStreamResult loRtn = null;
            try
            {
                //Get Parameter
                _AllGLTR00100Parameter = R_NetCoreUtility.R_DeserializeObjectFromByte<GLTR00100PrintParamDTO>(R_DistributedCache.Cache.Get(pcGuid));
                loRtn = new FileStreamResult(_ReportCls.R_GetStreamReport(), R_ReportUtility.GetMimeType(R_FileType.PDF));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        #region Helper
        private GLTR00100ResultWithBaseHeaderPrintDTO GenerateDataPrint(GLTR00100PrintParamDTO poParam)
        {
            var loEx = new R_Exception();
            GLTR00100ResultWithBaseHeaderPrintDTO loRtn = new GLTR00100ResultWithBaseHeaderPrintDTO();

            try
            {
                var loCls = new GLTR00100Cls();

                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParam.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;

                var loDetailData = loCls.GetReportJournalTransaction(poParam);

                // set Base Header Common
                var loParam = new BaseHeaderDTO()
                {
                    CCOMPANY_NAME = "JOURNAL TRANSACTION",
                    CPRINT_CODE = "001",
                    CPRINT_NAME = "Utility Charges",
                    CUSER_ID = "FMC",
                };

                // get image
                //Assembly loAsm = Assembly.Load("BIMASAKTI_GL_API");
                //var lcResourceFile = "BIMASAKTI_GL_API.Image.CompanyLogo.png";
                //using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                //{
                //    var ms = new MemoryStream();
                //    resFilestream.CopyTo(ms);
                //    var bytes = ms.ToArray();

                //    loParam.BLOGO_COMPANY = bytes;
                //}

                GLTR00100ResultPrintDTO loData = new GLTR00100ResultPrintDTO();

                // Set Header Data
                var loHeaderData = R_Utility.R_ConvertObjectToObject<GLTR00100PrintDTO, GLTR00101DTO>(loDetailData.FirstOrDefault());

                //Convert Header Data
                loHeaderData.DDOC_DATE = !string.IsNullOrWhiteSpace(loHeaderData.CDOC_DATE)
                                        ? DateTime.ParseExact(loHeaderData.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                                        : default;
                loHeaderData.DREF_DATE = !string.IsNullOrWhiteSpace(loHeaderData.CREF_DATE)
                                        ? DateTime.ParseExact(loHeaderData.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                                        : default;
                loHeaderData.DREVERSE_DATE = !string.IsNullOrWhiteSpace(loHeaderData.CREVERSE_DATE)
                                        ? DateTime.ParseExact(loHeaderData.CREVERSE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                                        : default;

                // Set Detail Data
                List<GLTR00102DTO> loDetail = new List<GLTR00102DTO>();
                foreach (var item in loDetailData)
                {
                    //Convert Detail Data
                    var itemDetail = new GLTR00102DTO
                    {
                        CGLACCOUNT_NO = item.CGLACCOUNT_NO,
                        CGLACCOUNT_NAME = item.CGLACCOUNT_NAME,
                        CCENTER_CODE = item.CCENTER_CODE,
                        CCENTER_NAME = item.CCENTER_NAME,
                        CDBCR = item.CDBCR,
                        NTRANS_AMOUNT = item.NTRANS_AMOUNT, // Example decimal value
                        CDETAIL_DESC = item.CDETAIL_DESC,
                        CDOCUMENT_NO = item.CDOCUMENT_NO,
                        CDOCUMENT_DATE = item.CDOCUMENT_DATE,
                        DDOCUMENT_DATE = !string.IsNullOrWhiteSpace(item.CDOCUMENT_DATE)
                                        ? DateTime.ParseExact(item.CDOCUMENT_DATE, "yyyyMMdd", CultureInfo.InvariantCulture)
                                        : default
                    };

                    loDetail.Add(itemDetail);
                }

                // Assign Data
                loData.HeaderData = loHeaderData;
                loData.ListDetail = loDetail;

                // Assign Data with Base Header Data
                loRtn.BaseHeaderData = loParam;
                loRtn.GLTR = loData;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        #endregion
    }
}