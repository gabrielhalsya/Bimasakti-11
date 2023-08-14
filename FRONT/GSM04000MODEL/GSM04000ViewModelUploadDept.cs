using GSM04000Common;
using R_APICommonDTO;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
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
        public ObservableCollection<GSM04000ExcelToUploadDTO> DepartmentExcelList { get; set; } = new ObservableCollection<GSM04000ExcelToUploadDTO>();
        public ObservableCollection<GSM04000DTO> DepartmentList { get; set; } = new ObservableCollection<GSM04000DTO>();

        GSM04000Model _model = new GSM04000Model();
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile = false;
        public bool _isOverwrite = false;
        public Action StateChangeAction { get; set; }
        public const int _batchStep = 12;
        public string _progressBarMessage = "";
        public int _progressBarPercentage = 0;
        public bool _showNotesErrorColumn = false;
        public bool _enableSaveButton = false;


        public async Task GetDepartmentToCompareList()
        {
            R_Exception loEx = new R_Exception();
            List<GSM04000DTO> loResult = null;
            try
            {
                loResult = new List<GSM04000DTO>();
                loResult = await _model.GetDeptDatatoCompareAsync();
                DepartmentList = new ObservableCollection<GSM04000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

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
                if (DepartmentExcelList.Count == 0)
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
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            switch (poProcessResultMode)
            {
                case eProcessResultMode.Success:
                    _progressBarMessage = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                    break;
                case eProcessResultMode.Fail:

                    break;
                default:
                    break;
            }
            StateChangeAction();
        }
        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            throw new NotImplementedException();
        }
        public async Task ReportProgress(int pnProgress, string pcStatus)
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
                loUploadPar.ClassName = "GSM04000Back.UploadClsSS";
                loUploadPar.BigObject = poBigObject;

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl");

                var loKeyGuid = await loCls.R_BatchProcess<List<GSM04000ExcelToUploadDTO>>(loUploadPar, _batchStep);
                DepartmentExcelList = new ObservableCollection<GSM04000ExcelToUploadDTO>(poBigObject);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        }
        #endregion

        public async Task ValidateDataList(List<GSM04000ExcelToUploadDTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await GetDepartmentToCompareList();

                var loObject = poEntity.Select(loTemp => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = loTemp.DepartmentCode,
                    CDEPT_NAME = loTemp.DepartmentName,
                    CCENTER_CODE = loTemp.CenterCode,
                    CMANAGER_NAME = loTemp.ManagerName,
                    LEVERYONE = loTemp.Everyone,
                    LACTIVE = loTemp.Active,
                    CNON_ACTIVE_DATE = loTemp.NonActiveDate,
                }).ToList();

                var comparedList = loObject.Join(DepartmentList, loDept1 => loDept1.CDEPT_CODE, loDept2 => loDept2.CDEPT_CODE, (loDept1, loDept2) => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = loDept1.CDEPT_CODE,
                    LEXISTS = true,  // Set to 1 when the code exists in both lists
                    LSELECTED = false,
                    LOVERWRITE = false
                }).Union(DepartmentList.Where(loDept2 => !loObject.Any(loDept1 => loDept1.CDEPT_CODE == loDept2.CDEPT_CODE)).Select(loDept2 => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = loDept2.CDEPT_CODE,
                    LEXISTS = false,
                    LSELECTED = true,
                    LOVERWRITE = false
                })).Union(loObject.Where(loDept1 => !DepartmentList.Any(loDept2 => loDept2.CDEPT_CODE == loDept1.CDEPT_CODE)).Select(loDept1 => new GSM04000ExcelGridDTO
                {
                    CDEPT_CODE = loDept1.CDEPT_CODE,
                    LEXISTS = false,
                    LSELECTED = true,
                    LOVERWRITE = false
                })).ToList();

                var loData = poEntity.Select(item =>
                {
                    //item.Var_Exists = loMasterData.Contains(item.StaffId);
                    return item;
                }).ToList();

                //await ConvertGrid(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
    }
}
