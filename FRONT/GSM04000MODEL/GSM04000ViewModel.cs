using GSM04000Common;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GSM04000Model
{
    public class GSM04000ViewModel : R_ViewModel<GSM04000DTO>, R_IProcessProgressStatus
    {
        private GSM04000Model _model = new GSM04000Model();
        public ObservableCollection<GSM04000DTO> DepartmentList { get; set; } = new ObservableCollection<GSM04000DTO>();
        public ObservableCollection<GSM04000ExcelGridDTO> DepartmentExcelList { get; set; } = new ObservableCollection<GSM04000ExcelGridDTO>();
        public GSM04000DTO Department { get; set; } = new GSM04000DTO();
        public R_ContextHeader _contextHeader { get; set; }
        public string _departmentCode { get; set; } = "";
        public bool _activeDept { get; set; }
        public bool _isUserDeptExist { get; set; }
        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile = false;
        public bool _isOverwrite = false;

        public async Task GetDepartmentList()
        {
            R_Exception loEx = new R_Exception();
            List<GSM04000DTO> loResult = null;
            try
            {
                loResult = new List<GSM04000DTO>();
                loResult = await _model.GetGSM04000ListAsync();
                DepartmentList = new ObservableCollection<GSM04000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetDepartment(GSM04000DTO poDept)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                GSM04000DTO loParam = new GSM04000DTO();
                loParam = poDept;
                var loResult = await _model.R_ServiceGetRecordAsync(loParam);
                Department = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveDepartment(GSM04000DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                poNewEntity.CMANAGER_NAME = poNewEntity.CMANAGER_CODE;
                var loResult = await _model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                Department = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task DeleteDepartment(GSM04000DTO poDept)
        {
            var loEx = new R_Exception();

            try
            {
                GSM04000DTO loParam = new GSM04000DTO();
                loParam = poDept;
                await _model.R_ServiceDeleteAsync(poDept);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task ActiveInactiveProcessAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, _departmentCode);
                R_FrontContext.R_SetContext(ContextConstant.LACTIVE, _activeDept);
                await _model.RSP_GS_ACTIVE_INACTIVE_DEPTMethodAsync();

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task CheckIsUserDeptExistAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, _departmentCode);
                var loResult = await _model.CheckIsUserDeptExistAsync();
                _isUserDeptExist = loResult.UserDeptExist;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task DeleteAssignedUserWhenChangeEveryone()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, _departmentCode);
                var loResult = await _model.DeleteDeptUserWhenChaningEveryoneAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #region Template
        public async Task<UploadFileDTO> DownloadTemplate()
        {
            var loEx = new R_Exception();
            UploadFileDTO loResult = null;

            try
            {
                loResult = await _model.DownloadTemplateDeptartmentAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

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
        #endregion
    }

}

