using GSM04000Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GSM04000Model
{
    public class GSM04000ViewModel : R_ViewModel<GSM04000DTO>
    {
        private GSM04000Model _model = new GSM04000Model();
        public ObservableCollection<GSM04000DTO> DepartmentList { get; set; } = new ObservableCollection<GSM04000DTO>();
        public ObservableCollection<GSM04000DTO> DepartmentExcelList { get; set; } = new ObservableCollection<GSM04000DTO>();

        public GSM04000DTO Department { get; set; } = new GSM04000DTO();
        public R_ContextHeader _ContextHeader { get; set; }
        public string DepartmentCode { get; set; } = "";
        public bool ActiveDept { get; set; }
        public bool IsUserDeptExist { get; set; }

        public string _sourceFileName { get; set; }
        public bool _isErrorEmptyFile = false;


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
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, DepartmentCode);
                R_FrontContext.R_SetContext(ContextConstant.LACTIVE, ActiveDept);
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
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, DepartmentCode);
                var loResult = await _model.CheckIsUserDeptExistAsync();
                IsUserDeptExist = loResult.UserDeptExist;
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
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, DepartmentCode);
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

    }

}

