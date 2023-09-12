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
using Microsoft.JSInterop;
using R_APICommonDTO;

namespace GSM04000Front
{
    public partial class GSM04000PopupUpload : R_Page
    {
        private GSM04000ViewModelUploadDept _deptUploadViewModel = new GSM04000ViewModelUploadDept();

        private R_Grid<GSM04000ExcelGridDTO> _gridDeptExcelRef;

        private R_ConductorGrid _conGridDeptExcelRef;

        [Inject] private R_IExcel _excelProvider { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }

        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Excel };

        public byte[] _fileByte = null;

        private bool _isUploadSuccesful = true;
        private bool _isFileHasData { get; set; }

        // Create Method Action StateHasChange
        private void StateChangeInvoke()
        {
            StateHasChanged();
        }

        // Create Method Action For Download Excel if Has Error
        private async Task ActionFuncDataSetExcel()
        {
            var loByte = _excelProvider.R_WriteToExcel(_deptUploadViewModel._ExcelDataset);
            var lcName = $"{_clientHelper.CompanyId}" + ".xlsx";

            await _JSRuntime.downloadFileFromStreamHandler(lcName, loByte);
        }

        // Create Method Action For Error Unhandle
        private void ShowErrorInvoke(R_APIException poEx)
        {
            var loEx = new R_Exception(poEx.ErrorList.Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.ErrNo, x.ErrDescp)).ToList());
            this.R_DisplayException(loEx);
        }

        protected override Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _deptUploadViewModel._stateChangeAction = StateChangeInvoke;
                _deptUploadViewModel._ShowErrorAction = ShowErrorInvoke;
                _deptUploadViewModel._ActionDataSetExcel = ActionFuncDataSetExcel;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
            return Task.CompletedTask;
        }
        
        private async Task UploadExcel(InputFileChangeEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _deptUploadViewModel._sourceFileName = eventArgs.File.Name;
                //import from excel
                var loMS = new MemoryStream();
                await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
                var loFileByte = loMS.ToArray();

                //add filebyte to DTO
                var loExcel = _excelProvider;

                var loDataSet = loExcel.R_ReadFromExcel(loFileByte, new string[] { "Department" });

                var loResult = R_FrontUtility.R_ConvertTo<GSM04000ExcelToUploadDTO>(loDataSet.Tables[0]);

                _isFileHasData = loResult.Count > 0 ? true : false;

                await _gridDeptExcelRef.R_RefreshGrid(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task DeptExcelGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (List<GSM04000ExcelToUploadDTO>)eventArgs.Parameter;
                await _deptUploadViewModel.ConvertGridExelToGridDTO(loData);
                eventArgs.ListEntityResult = _deptUploadViewModel._DepartmentExcelGridData;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        public async Task OnClick_ButtonOK()
        {
            var loEx = new R_Exception();
            try
            {
                var loValidate = await R_MessageBox.Show("", "Are you sure want to import data?", R_eMessageBoxButtonType.YesNo);

                if (loValidate == R_eMessageBoxResult.Yes)
                {
                    //await _deptUploadViewModel.SaveBatch_UploadValidatedData(_clientHelper.CompanyId, _clientHelper.UserId);
                    await this.Close(true, true);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task OnClick_ButtonSaveExcelAsync()
        {
            var loEx = new R_Exception();
            try
            {
                //var loValidate = await R_MessageBox.Show("", "Are you sure want to import data?", R_eMessageBoxButtonType.YesNo);

                //if (loValidate == R_eMessageBoxResult.Yes)
                //{
                //    await _deptUploadViewModel.SaveBatch_UploadValidatedData(_clientHelper.CompanyId, _clientHelper.UserId);
                //    await this.Close(true, true);
                //}
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
