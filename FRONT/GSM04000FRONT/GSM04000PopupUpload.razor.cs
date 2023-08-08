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
        private R_Grid<GSM04000DTO> _gridDeptExcelRef;
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

        private async Task DeptGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _deptViewModel.GetDepartmentList();
                eventArgs.ListEntityResult = _deptViewModel.DepartmentList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        private async Task UploadExcel(InputFileChangeEventArgs eventArgs)
        {
            //var loMS = new MemoryStream();
            //await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
            //var loByteFile = loMS.ToArray();

            ////import from excel
            //var loDataSet = _excelProvider.R_ReadFromExcel(loByteFile);

            //var resultEmployee = R_FrontUtility.R_ConvertTo<GSM04000DTO>(loDataSet.Tables[0]);
            //ObservableCollection<GSM04000DTO> listEmployee = new ObservableCollection<GSM04000DTO>(resultEmployee);
            //_deptViewModel.DepartmentExcelList = listEmployee;

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
                //await JournalGroup_gridRef.R_RefreshGrid(null);
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

        private void ReadExcelFile()
        {
            var loEx = new R_Exception();
            List<GSM04000ExcelDTO> loExtract = new List<GSM04000ExcelDTO>();
            try
            {
                //Read From EXCEL
                var loExcel = new R_Excel();
                var loDataSet = loExcel.R_ReadFromExcel(_fileByte, new string[] { "Department" });
                var loResult = R_FrontUtility.R_ConvertTo<GSM04000ExcelDTO>(loDataSet.Tables[0]);
                loExtract = new List<GSM04000ExcelDTO>(loResult);

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
