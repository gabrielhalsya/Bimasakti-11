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
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GSM04000Model
{
    public class GSM04000ViewModelUploadDept : R_IProcessProgressStatus
    {
        public const int NBATCH_STEP = 11;
        GSM04000Model _model = new GSM04000Model();
        public ObservableCollection<GSM04000ExcelGridDTO> _DepartmentExcelGridData { get; set; } = new ObservableCollection<GSM04000ExcelGridDTO>();
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile { get;set; } =false;
        public Action _stateChangeAction { get; set; }
        public Action<R_APIException> _ShowErrorAction { get; set; }
        public Func<Task> _ActionDataSetExcel { get; set; }
        public DataSet _ExcelDataset { get; set; }

        public string _progressBarMessage = "";
        public int _progressBarPercentage = 0;
        public int _sumValidDataDeptExcel { get; set; } = 0;
        public int _sumListDeptExcel { get; set; } = 0;
        public int _sumInvalidDataDeptExcel { get; set; } = 0;
        public bool _visibleError { get; set; }=false;

        public async Task ConvertGridExelToGridDTO(List<GSM04000ExcelToUploadDTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                // Onchange Visible Error
                _visibleError = false;
                _sumValidDataDeptExcel = 0;
                _sumInvalidDataDeptExcel = 0;

                // Convert Excel DTO and add SeqNo
                var loData = poEntity.Select((loTemp, i) => new GSM04000ExcelGridDTO
                {
                    INO = i + 1,//add sequence
                    CDEPT_CODE = loTemp.DepartmentCode,
                    CDEPT_NAME = loTemp.DepartmentName,
                    CCENTER_CODE = loTemp.CenterCode,
                    CMANAGER_CODE = loTemp.ManagerName,
                    LACTIVE = loTemp.Active,
                    LEVERYONE = loTemp.Everyone,
                    CNON_ACTIVE_DATE = loTemp.NonActiveDate,
                    CNOTES = loTemp.Notes,
                    DNON_ACTIVE_DATE_DISPLAY = !string.IsNullOrWhiteSpace(loTemp.NonActiveDate) ? DateTime.ParseExact(loTemp.NonActiveDate, "yyyyMMdd", CultureInfo.InvariantCulture) : default,//create 1 property to display date
                }).ToList();

                //count
                _sumListDeptExcel = loData.Count;

                //assign to grid object
                _DepartmentExcelGridData = new ObservableCollection<GSM04000ExcelGridDTO>(loData);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        
        public async Task SaveBulk_DeptExcelData(List<GSM04000ExcelToUploadDTO> poBigObject, string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04000ExcelToUploadDTO> Bigobject;
            List<R_KeyValue> loUserParameneters;
            //R_IProcessProgressStatus loProgressStatus;
            try
            {
                
                //set param
                loUserParameneters = new List<R_KeyValue>();
                
                //instance processclient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl");

                //assign data
                if (_DepartmentExcelGridData.Count<=0)
                {
                    return;
                }
                Bigobject = poBigObject;

                loBatchPar = new R_BatchParameter();
                loBatchPar.COMPANY_ID = pcCompanyId;
                loBatchPar.USER_ID = pcUserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM04000Back.GSM00400UploadCls";
                loBatchPar.BigObject = Bigobject;


                var loKeyGuid = await loCls.R_BatchProcess<List<GSM04000ExcelToUploadDTO>>(loBatchPar, NBATCH_STEP);
                //DepartmentExcelData = new ObservableCollection<GSM04000ExcelToUploadDTO>(poBigObject);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        
        #region BatchProcess
        
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            switch (poProcessResultMode)
            {
                case eProcessResultMode.Success:
                    //shown message
                    _progressBarMessage = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);

                    //enable save button and hide errorcolumn
                    //_enableSaveButton = true;
                    //_showNotesErrorColumn = false;

                    //convert & assign data to binded grid data
                    //await ValidateDataList(DepartmentExcelData);

                    break;
                case eProcessResultMode.Fail:
                    //_enableSaveButton = false;
                    //_showNotesErrorColumn = true;
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
        private async Task GetError(string pcKeyGuid)
        {
            R_APIException loException;
            try
            {
                var loError = await _model.GetErrorProcessAsync(pcKeyGuid);
                foreach (var item in _DepartmentExcelGridData)
                {
                    item.CNOTES = loError.Where(x => x.CDEPT_CODE == item.CDEPT_CODE).Select(x => x.CNOTES).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
