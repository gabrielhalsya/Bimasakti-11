using BlazorClientHelper;
using GSM04000Common;
using GSM04000Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Controls.Grid.Columns;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System.Net.Http.Headers;

namespace GSM04000Front
{
    public partial class GSM04000 : R_Page
    {
        private GSM04000ViewModel _deptViewModel = new GSM04000ViewModel();
        private R_Grid<GSM04000DTO> _gridDeptRef;
        private R_ConductorGrid _conGridDeptRef;

        private GSM04100ViewModel _deptUserViewModel = new GSM04100ViewModel();
        private R_Grid<GSM04100StreamDTO> _gridDeptUserRef;
        private R_ConductorGrid _conGridDeptUserRef;

        [Inject] private R_IExcel _excelProvider { get; set; }
        [Inject] IClientHelper _clientHelper { get; set; }

        private R_Popup R_PopupAssignUser;

        private R_Popup R_PopupActiveInactive;

        private string loLabelActiveInactive = "Active/Inactive";
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridDeptRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        #region Department(PARENT)
        private async Task DeptGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _deptViewModel.GetDepartmentList();
                eventArgs.ListEntityResult = _deptViewModel.DepartmentList;
                if (eventArgs.ListEntityResult == null)
                {
                    R_PopupActiveInactive.Enabled = false;
                    R_PopupAssignUser.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task DeptGrid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM04000DTO)eventArgs.Data;
                var loDeptode = (GSM04100DTO)_conGridDeptUserRef.R_GetCurrentData();
                loDeptode.CDEPT_CODE = loParam.CDEPT_CODE;
                loDeptode.CDEPT_NAME = loParam.CDEPT_NAME;
                //set to viewmodel parent
                _deptViewModel._departmentCode = loParam.CDEPT_CODE;
                _deptViewModel._activeDept = loParam.LACTIVE;

                if (loParam.LACTIVE)
                {
                    loLabelActiveInactive = "Inactive";
                    _deptViewModel._activeDept = false;
                }
                else
                {
                    loLabelActiveInactive = "Activate";
                    _deptViewModel._activeDept = true;
                }
                //set to view model child
                _deptUserViewModel.DepartmentCode = loParam.CDEPT_CODE;
                if (loParam.LEVERYONE == true)
                {
                    R_PopupAssignUser.Enabled = false;
                }
                else
                {
                    R_PopupAssignUser.Enabled = true;
                }

                await _gridDeptUserRef.R_RefreshGrid(loParam);
            }

            if (eventArgs.ConductorMode == R_eConductorMode.Edit)
            {
                var loParam = (GSM04000DTO)eventArgs.Data;
                if (loParam.LEVERYONE != true)
                {
                    await R_MessageBox.Show("Confirmation", "changing this will delete all assinged user on this dept", R_eMessageBoxButtonType.OKCancel);
                }
            }
        }
        private async Task DeptGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM04000DTO>(eventArgs.Data);
                await _deptViewModel.GetDepartment(loParam);

                eventArgs.Result = _deptViewModel.Department;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task DeptGrid_Validation(R_ValidationEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (GSM04000DTO)eventArgs.Data;
                if (_deptViewModel.Department.LEVERYONE == false && loData.LEVERYONE == true)
                {
                    await _deptViewModel.CheckIsUserDeptExistAsync();
                    if (_deptViewModel._isUserDeptExist)
                    {
                        var loConfirm = await R_MessageBox.Show("Delete Confirmation", "Changing Value Everyone will delete User for this Department", R_eMessageBoxButtonType.OKCancel);
                        if (loConfirm == R_eMessageBoxResult.Cancel)
                        {
                            eventArgs.Cancel = true;
                        }
                        await _deptViewModel.DeleteAssignedUserWhenChangeEveryone();
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;
            loEx.ThrowExceptionIfErrors();
        }
        private async Task DeptGrid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _deptViewModel.SaveDepartment((GSM04000DTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _deptViewModel.Department;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task DeptGrid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (GSM04000DTO)eventArgs.Data;

                await _deptViewModel.DeleteDepartment(loData);
                await _gridDeptRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion//Department(PARENT)

        #region GridLookup
        private void Dept_Before_Open_Lookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {

            //membedakan columname dan mengarahkan tampil lookup
            switch (eventArgs.ColumnName)
            {
                case "CCENTER":
                    eventArgs.Parameter = new GSL00900ParameterDTO();
                    eventArgs.TargetPageType = typeof(GSL00900);
                    break;
                case "CMANAGER_NAME":
                    eventArgs.TargetPageType = typeof(GSL01000);
                    break;
            }

        }
        private void Dept_After_Open_Lookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            //mengambil result dari popup dan set ke data row
            if (eventArgs.Result == null)
            {
                return;
            }
            switch (eventArgs.ColumnName)
            {
                case "CCENTER":
                    var loTempResult = R_FrontUtility.ConvertObjectToObject<GSL00900DTO>(eventArgs.Result);
                    ((GSM04000DTO)eventArgs.ColumnData).CCENTER_CODE = loTempResult.CCENTER_CODE;
                    break;
                case "CMANAGER_NAME":
                    var loTempResult2 = R_FrontUtility.ConvertObjectToObject<GSL01000DTO>(eventArgs.Result);
                    ((GSM04000DTO)eventArgs.ColumnData).CMANAGER_CODE = loTempResult2.CUSER_ID;
                    break;
            }
        }
        #endregion//GridLookup

        #region DepartmentUser(CHILD)
        private async Task DeptUserGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM04000DTO>(eventArgs.Parameter);
                _deptUserViewModel.DepartmentCode = loParam.CDEPT_CODE;
                await _deptUserViewModel.GetDepartmentListByDeptCode();
                eventArgs.ListEntityResult = _deptUserViewModel.DepartmentUserList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task DeptUserGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM04100DTO>(eventArgs.Data);
                loParam.CDEPT_CODE = _deptUserViewModel.DepartmentCode;
                await _deptUserViewModel.GetDepartmentUser(loParam);
                eventArgs.Result = _deptUserViewModel.UserToAssign;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task DeptUserGrid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (GSM04100StreamDTO)eventArgs.Data;
                await _deptUserViewModel.DeleteUserDepartment(loData);
                await _gridDeptUserRef.R_RefreshGrid(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Assign User
        private void R_Before_Open_PopupAssignUser(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM04000PopupAssignUser);
        }
        private async Task R_After_Open_PopupAssignUser(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loTempResult = (GSM04100DTO)eventArgs.Result;
            var loDeptode = (GSM04100DTO)_conGridDeptUserRef.R_GetCurrentData();
            if (loTempResult == null)
            {
                return;
            }
            await _deptUserViewModel.AssignUserToDept(new GSM04100DTO()
            {
                CDEPT_CODE = loDeptode.CDEPT_CODE,
                CUSER_ID = loTempResult.CUSER_ID
            },
            eCRUDMode.AddMode);
            await _gridDeptUserRef.R_RefreshGrid(loDeptode);
        }
        #endregion

        #region Active/Inactive
        private void R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = "GSM04001";
            eventArgs.TargetPageType = typeof(GFF00900FRONT.GFF00900);
        }
        private async Task R_After_Open_Popup_ActivateInactive(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                bool result = (bool)eventArgs.Result;
                if (result == true)
                {
                    await _deptViewModel.ActiveInactiveProcessAsync();
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            await _gridDeptRef.R_RefreshGrid(null);
        }
        #endregion

        #region Upload
        private void R_Before_Open_PopupUpload(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM04000PopupUpload);
        }
        #endregion

        #region Rowrender
        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (GSM04000DTO)eventArgs.Data;

            //if (loData.GenderId == "M")
            // {
            //     eventArgs.RowStyle = new R_GridRowRenderStyle
            //     {
            //         FontColor = "red"
            //     };
            // }
        }
        #endregion

        #region Template
        private async Task DownloadTemplateAsync()
        {

            var loEx = new R_Exception();
            try
            {
                var loValidate = await R_MessageBox.Show("", "Are you sure download this template?", R_eMessageBoxButtonType.YesNo);

                if (loValidate == R_eMessageBoxResult.Yes)
                {
                    var loByteFile = await _deptViewModel.DownloadTemplate();

                    var saveFileName = $"{_clientHelper.CompanyId}.xlsx";

                    await JS.downloadFileFromStreamHandler(saveFileName, loByteFile.FileBytes);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            R_DisplayException(loEx);
        }
        #endregion
    }
}
