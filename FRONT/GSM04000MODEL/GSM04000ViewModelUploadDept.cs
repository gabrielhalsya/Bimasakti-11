using GSM04000Common;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GSM04000Model
{
    public class GSM04000ViewModelUploadDept : R_IProcessProgressStatus
    {
        public ObservableCollection<GSM04000ExcelGridDTO> DepartmentExcelList { get; set; } = new ObservableCollection<GSM04000ExcelGridDTO>();
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile = false;
        public bool _isOverwrite = false;

        public const int _batchStep = 12;

        public string _progressBarMessage = "";

        public int _progressBarPercentage = 0;

        #region Upload
        public async Task SaveFileBulkFile(string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04000ExcelGridDTO> ListFromExcel;
            List<R_KeyValue> loUserParameneters = null;

            try
            {
                // set Param
                loUserParameneters = new List<R_KeyValue>();
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.LOVERWRITE, Value = _isOverwrite });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl",
                    poProcessProgressStatus: this);

                //Set Data
                //if (JournalGroupValidateUploadError.Count == 0)
                //return;

                //ListFromExcel = JournalGroupValidateUploadError.ToList();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = pcCompanyId;
                loBatchPar.USER_ID = pcUserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM04000Back.GSM04000UploadCls";
                //loBatchPar.BigObject = ListFromExcel;
                //await loCls.R_BatchProcess<List<GSM04500UploadErrorValidateDTO>>(loBatchPar, ListFromExcel.Count);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            throw new NotImplementedException();
        }
        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            throw new NotImplementedException();
        }
        public Task ReportProgress(int pnProgress, string pcStatus)
        {
            throw new NotImplementedException();
        }
        public async Task AttachFile(List<GSM04000ExcelToUploadDTO> poBigObject, string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loUploadPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04000ExcelToUploadDTO> Bigobject;
            List<R_KeyValue> loUserParameneters;
            R_IProcessProgressStatus loProgressStatus;

            try
            {
                loUserParameneters = new List<R_KeyValue>();
                //loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = PropertyValue });

                //preapare Batch Parameter
                loUploadPar = new R_BatchParameter();
                loUploadPar.COMPANY_ID = pcCompanyId;
                loUploadPar.USER_ID = pcUserId;
                loUploadPar.UserParameters = loUserParameneters;
                loUploadPar.ClassName = "GSM04000Back.LMM06500UploadStaffCls";
                loUploadPar.BigObject = poBigObject;

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl");

                var loKeyGuid = await loCls.R_BatchProcess<List<GSM04000ExcelToUploadDTO>>(loUploadPar, _batchStep);

                //DepartmentExcelList = new ObservableCollection<GSM04000ExcelToReadDTO>(poBigObject);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        }
        #endregion
    }
}
