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
        private GSM04000ViewModel _deptViewModel = new GSM04000ViewModel();
        private R_Grid<GSM04000ExcelGridDTO> _gridDeptExcelRef;
        private R_ConductorGrid _conGridDeptExcelRef;
        [Inject] private R_IExcel _excelProvider { get; set; }
        [Inject] IClientHelper _clientHelper { get; set; }

        private R_eFileSelectAccept[] _accepts = { R_eFileSelectAccept.Excel };
        public byte[] _fileByte = null;
        private bool _isFileExist = false;
        private bool _isUploadSuccesful = true;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridDeptExcelRef.R_RefreshGrid(null);
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
                await _deptViewModel.AttachFile(loData, _clientHelper.CompanyId, _clientHelper.UserId);
                eventArgs.ListEntityResult = _deptViewModel.DepartmentExcelList;
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
                _deptViewModel._sourceFileName = eventArgs.File.Name;

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
                if (_deptViewModel._isErrorEmptyFile)
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
                var loExcel = new R_Excel();
                var loDataSet = loExcel.R_ReadFromExcel(_fileByte, new string[] { "Department" });
                var loResult = R_FrontUtility.R_ConvertTo<GSM04000ExcelToUploadDTO>(loDataSet.Tables[0]);
                loExtract = new List<GSM04000ExcelToUploadDTO>(loResult);

                //refresh grid
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
            var loData = (GSM04000DTO)eventArgs.Data;

            if (loData.LACTIVE)
            {
                eventArgs.RowStyle = new R_GridRowRenderStyle
                {
                    FontColor = "red"
                };
            }
        }
    }
}
