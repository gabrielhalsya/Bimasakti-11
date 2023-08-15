using GSM04000Common;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSM04000Model
{
    public class GSM04000ViewModelUploadDept : R_IProcessProgressStatus
    {
        GSM04000Model _model = new GSM04000Model();
        public ObservableCollection<GSM04000ExcelGridDTO> DepartmentExcelValidatedData { get; set; } = new ObservableCollection<GSM04000ExcelGridDTO>();
        public ObservableCollection<GSM04000ExcelToUploadDTO> DepartmentExcelData { get; set; } = new ObservableCollection<GSM04000ExcelToUploadDTO>();
        public ObservableCollection<GSM04000DTO> CurrentDepartmentData { get; set; } = new ObservableCollection<GSM04000DTO>();
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile = false;
        public bool _isOverwrite = false;
        public Action _stateChangeAction { get; set; }
        public const int NBATCH_STEP = 12;
        public string _progressBarMessage = "";
        public int _progressBarPercentage = 0;
        public bool _showNotesErrorColumn = false;
        public bool _enableSaveButton { get; set; } = false;

        public async Task GetDepartmentToCompareList()
        {
            R_Exception loEx = new R_Exception();
            List<GSM04000DTO> loResult = null;
            try
            {
                loResult = new List<GSM04000DTO>();
                loResult = await _model.GetDeptDataToCompareAsync();
                CurrentDepartmentData = new ObservableCollection<GSM04000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task ValidateDataList(ObservableCollection<GSM04000ExcelToUploadDTO> poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                //get department master
                await GetDepartmentToCompareList();

                //parsing parameter to list in order to compare
                var loListDeptFromExcel = poEntity.Select(loTemp => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = loTemp.DepartmentCode,
                    CDEPT_NAME = loTemp.DepartmentName,
                    CCENTER_CODE = loTemp.CenterCode,
                    CMANAGER_NAME = loTemp.ManagerName,
                    LEVERYONE = loTemp.Everyone,
                    LACTIVE = loTemp.Active,
                    CNON_ACTIVE_DATE = loTemp.NonActiveDate,
                }).ToList();

                //compare list
                var loComparedData = CurrentDepartmentData.Union(loListDeptFromExcel).GroupBy(loDept => loDept.CDEPT_CODE).Select(group => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = group.Key,
                    LEXISTS = group.Count() > 1,
                    LSELECTED = group.Count() > 1,
                    LOVERWRITE = false
                }).ToList();

                //assign data to list grid binding
                DepartmentExcelValidatedData = new ObservableCollection<GSM04000ExcelGridDTO>(loComparedData);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        private async Task GetError(string pcKeyGuid)
        {
            R_APIException loException;
            try
            {
                var loError = await _model.GetErrorProcessAsync(pcKeyGuid);
                foreach (var item in DepartmentExcelValidatedData)
                {
                    item.CERROR_MESSAGE = loError.Where(x => x.CDEPT_CODE == item.CDEPT_CODE).Select(x => x.CERROR_MESSAGE).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
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
                if (DepartmentExcelData.Count == 0)
                    return;

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
                //loUploadPar.UserParameters = loUserParameneters;
                loUploadPar.ClassName = "GSM04000Back.GSM04000UploadValidationCls";
                loUploadPar.BigObject = poBigObject;

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl");

                var loKeyGuid = await loCls.R_BatchProcess<List<GSM04000ExcelToUploadDTO>>(loUploadPar, NBATCH_STEP);
                DepartmentExcelData = new ObservableCollection<GSM04000ExcelToUploadDTO>(poBigObject);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        }

        #region Upload
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            switch (poProcessResultMode)
            {
                case eProcessResultMode.Success:
                    //shown message
                    _progressBarMessage = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                    
                    //enable save button and hide errorcolumn
                    _enableSaveButton = true;
                    _showNotesErrorColumn = false;

                    //convert & assign data to binded grid data
                    await ValidateDataList(DepartmentExcelData);

                    break;
                case eProcessResultMode.Fail:
                    _enableSaveButton=false;
                    _showNotesErrorColumn = true;
                    _progressBarMessage = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
                    await GetError(pcKeyGuid);
                    break;
                default:
                    break;
            }
            _stateChangeAction();
        }
        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            foreach (R_APICommonDTO.R_Error loerror in ex.ErrorList)
            {
                _progressBarMessage = string.Format($"{loerror.ErrDescp}");
            }
            _stateChangeAction();
            await Task.CompletedTask;
        }
        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            _progressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);
            _progressBarPercentage = pnProgress;
            _progressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);
            _stateChangeAction();
            await Task.CompletedTask;
        }
        #endregion

    }
}
