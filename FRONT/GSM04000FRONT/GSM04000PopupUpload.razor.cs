using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using GSM04000Common;
using GSM04000Model;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Helpers;
using System;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd;
using Microsoft.AspNetCore.Components.Forms;
using R_BlazorFrontEnd.Controls.Enums;
using System.Collections.ObjectModel;
using R_BlazorFrontEnd.Controls.Grid;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Excel;
using BlazorClientHelper;

namespace GSM04000Front
{
    public partial class GSM04000PopupUpload : R_Page
    {
        private GSM04000ViewModelUploadDept _deptUploadViewModel = new GSM04000ViewModelUploadDept();

        private R_Grid<GSM04000ExcelGridDTO> _gridDeptExcelRef;

        private R_ConductorGrid _conGridDeptExcelRef;

        [Inject] private R_IExcel _excelProvider { get; set; }

        [Inject] IClientHelper _clientHelper { get; set; }

        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Excel };

        public byte[] _fileByte = null;

        private bool _isFileExist = false;

        private bool _isUploadSuccesful = true;

        private void StateChangeInvoke()
        {
            StateHasChanged();
        }

        protected override  Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _deptUploadViewModel._stateChangeAction = StateChangeInvoke;
                //await _gridDeptExcelRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
            return Task.CompletedTask;
        }

        private async Task DeptExcelGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (List<GSM04000ExcelToUploadDTO>)eventArgs.Parameter;
                await _deptUploadViewModel.AttachFile(loData, _clientHelper.CompanyId, _clientHelper.UserId);
                eventArgs.ListEntityResult = _deptUploadViewModel.DepartmentExcelValidatedData;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        private async Task UploadExcel(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //get file name
                _deptUploadViewModel._sourceFileName = eventArgs.File.Name;

                //import excel from user
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                _fileByte = loMS.ToArray();

                //validate type file
                if (eventArgs.File.Name.Contains(".xlsx") == false)
                {
                    await R_MessageBox.Show("", "File Type must Microsoft Excel .xlsx", R_eMessageBoxButtonType.OK);
                }
                if (eventArgs.File.Name.Length > 0)
                {
                    _isFileExist = true;
                }
                else
                {
                    _isFileExist = false;
                }

                ReadExcelFile();
            }
            catch (Exception ex)
            {
                if (_deptUploadViewModel._isErrorEmptyFile)
                {   
                    await R_MessageBox.Show("", "File is Empty", R_eMessageBoxButtonType.OK);
                }
                else
                {
                    loEx.Add(ex);
                }
            }
        B:
            loEx.ThrowExceptionIfErrors();
        }

        private async Task ReadExcelFile()
        {
            var loEx = new R_Exception();
            List<GSM04000ExcelToUploadDTO> loExtract = new List<GSM04000ExcelToUploadDTO>();
            try
            {
                //Read From EXCEL
                var loDataSet = _excelProvider.R_ReadFromExcel(_fileByte, new string[] { "Department" });
                var loResult = R_FrontUtility.R_ConvertTo<GSM04000ExcelToUploadDTO>(loDataSet.Tables[0]);
                loExtract = new List<GSM04000ExcelToUploadDTO>(loResult);

                //Refresh grid
                await _gridDeptExcelRef.R_RefreshGrid(loExtract);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private void R_RowRender(R_GridRowRenderEventArgs eventArgs)
        {
            var loData = (GSM04000ExcelGridDTO)eventArgs.Data;

            if (loData.LEXISTS)
            {
                eventArgs.RowStyle = new R_GridRowRenderStyle
                {
                    FontColor = "red"
                };
            }
        }

        public async Task OnClick_ButtonOK()
        {
            var loEx = new R_Exception();
            try
            {
                var loValidate = await R_MessageBox.Show("", "Are you sure want to import data?", R_eMessageBoxButtonType.YesNo);

                if (loValidate == R_eMessageBoxResult.Yes)
                {
                    await _deptUploadViewModel.SaveFileBulkFile(_clientHelper.CompanyId, _clientHelper.UserId);
                    await this.Close(true, true);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task OnClick_ButtonClose()
        {
            await this.Close(true, false);
        }
    }
}
