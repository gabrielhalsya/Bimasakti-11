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
        private const string DEFAULT_MODULE = "GS";
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_CLASSNAME = "GSM04000Back.GSM00400UploadCls";

        GSM04000Model _model = new GSM04000Model();
        public ObservableCollection<GSM04000ExcelGridDTO> _DepartmentExcelGridData { get; set; } = new ObservableCollection<GSM04000ExcelGridDTO>();
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile { get; set; } = false;
        public Action _stateChangeAction { get; set; }
        public Action<R_APIException> _ShowErrorAction { get; set; }
        public Func<Task> _ActionDataSetExcel { get; set; }
        public DataSet _ExcelDataset { get; set; }

        public string _progressBarMessage = "";
        public int _progressBarPercentage = 0;
        public string _ccompanyId { get; set; }
        public string _cuserId { get; set; }

        public int _sumValidDataDeptExcel { get; set; } = 0;
        public int _sumListDeptExcel { get; set; } = 0;
        public int _sumInvalidDataDeptExcel { get; set; } = 0;
        public bool _visibleError { get; set; } = false;

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

        public async Task SaveBulk_DeptExcelData()
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<GSM04000ExcelGridDTO> Bigobject;
            List<R_KeyValue> loUserParameneters;
            //R_IProcessProgressStatus loProgressStatus;
            try
            {
                //set param
                loUserParameneters = new List<R_KeyValue>();

                //instance processclient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: DEFAULT_MODULE,
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: DEFAULT_HTTP_NAME);

                //assign data
                if (_DepartmentExcelGridData.Count <= 0)
                {
                    return;
                }
                Bigobject = _DepartmentExcelGridData.ToList<GSM04000ExcelGridDTO>();

                loBatchPar = new R_BatchParameter();
                loBatchPar.COMPANY_ID = _ccompanyId;
                loBatchPar.USER_ID = _cuserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = DEFAULT_CLASSNAME;
                loBatchPar.BigObject = Bigobject;


                var loKeyGuid = await loCls.R_BatchProcess<List<GSM04000ExcelGridDTO>>(loBatchPar, NBATCH_STEP);
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
            var loEx = new R_Exception();

            try
            {
                if (poProcessResultMode == eProcessResultMode.Success)
                {
                    _progressBarMessage = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                    _visibleError = false;
                }

                if (poProcessResultMode == eProcessResultMode.Fail)
                {
                    _progressBarMessage = $"Process Complete but fail with GUID {pcKeyGuid}";
                    await GetError(pcKeyGuid);
                    _visibleError = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            // Call Method Action StateHasChange
            _stateChangeAction();
            loEx.ThrowExceptionIfErrors();
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            _progressBarMessage = string.Format("Process Error with GUID {0}", pcKeyGuid);

            // Call Method Action Error Unhandle
            _ShowErrorAction(ex);

            // Call Method Action StateHasChange
            _stateChangeAction();

            await Task.CompletedTask;
        }

        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            _progressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            _progressBarPercentage = pnProgress;
            _progressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            // Call Method Action StateHasChange
            _stateChangeAction();

            await Task.CompletedTask;
        }

        #endregion
        private async Task GetError(string pcKeyGuid)
        {
            R_Exception loException = new R_Exception();

            List<R_ErrorStatusReturn> loResultData;
            R_GetErrorWithMultiLanguageParameter loParameterData;
            R_ProcessAndUploadClient loCls;

            try
            {
                // Add Parameter
                loParameterData = new R_GetErrorWithMultiLanguageParameter()
                {
                    COMPANY_ID = _ccompanyId,
                    USER_ID = _cuserId,
                    KEY_GUID = pcKeyGuid,
                };

                loCls = new R_ProcessAndUploadClient(pcModuleName: DEFAULT_MODULE,
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: DEFAULT_HTTP_NAME);

                // Get error result
                loResultData = await loCls.R_GetStreamErrorProcess(loParameterData);

                // check error if unhandle
                if (loResultData.Any(y => y.SeqNo <= 0))
                {
                    var loUnhandleEx = loResultData.Where(y => y.SeqNo <= 0).Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.SeqNo.ToString(), x.ErrorMessage)).ToList();
                    loUnhandleEx.ForEach(x => loException.Add(x));
                }

                if (loResultData.Any(y => y.SeqNo > 0))
                {
                    // Display Error Handle if get seq
                    _DepartmentExcelGridData.ToList().ForEach(x =>
                    {
                        //Assign ErrorMessage, Valid and Set Valid And Invalid Data
                        if (loResultData.Any(y => y.SeqNo == x.INO))
                        {
                            x.CNOTES = loResultData.Where(y => y.SeqNo == x.INO).FirstOrDefault().ErrorMessage;
                            x.CVALID = "N";
                            _sumInvalidDataDeptExcel++;
                        }
                        else
                        {
                            x.CVALID = "Y";
                            _sumValidDataDeptExcel++;
                        }
                    });

                    //Set DataSetTable and get error
                    var loExcelData = R_FrontUtility.ConvertCollectionToCollection<GSM04000ExcelGridDTO>(_DepartmentExcelGridData);

                    var loDataTable = R_FrontUtility.R_ConvertTo<GSM04000ExcelGridDTO>(loExcelData);
                    loDataTable.TableName = "Department";

                    var loDataSet = new DataSet();
                    loDataSet.Tables.Add(loDataTable);

                    // Asign Dataset
                    _ExcelDataset = loDataSet;

                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

    }
}
