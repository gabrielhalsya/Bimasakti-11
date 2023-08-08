using GLM00200Common;
using GLM00200Common.DTO_s;
using GLM00200Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;

namespace GLM00200Front
{
    public partial class GLM00200 : R_Page
    {
        private GLM00200ViewModel _journalVM = new GLM00200ViewModel();
        private R_Grid<JournalGridDTO> _gridJournal;
        private R_ConductorGrid _conJournal;

        private R_Grid<JournalDetailGridDTO> _gridJournalDet;
        private R_ConductorGrid _conJournalDet;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.GetFirstDepartData();
                await _journalVM.GetVAR_GSM_COMPANY_DTOAsync();
                await _journalVM.GetVAR_GL_SYSTEM_PARAMAsync();
                await _journalVM.GetCCURRENT_PERIOD_START_DATEAsync();
                await _journalVM.GetCSOFT_PERIOD_START_DATEAsync();
                await _journalVM.GetGSM_PERIODAsync();
                await _journalVM.GetGSM_TRANSACTION_CODEAsync();
                await _journalVM.GetIUNDO_COMMIT_JRNAsync();
                await _journalVM.GetSTATUS_DTOAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region TAB Predefined
        private void Predef_RecurringEntry(R_InstantiateDockEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(RecurringEntry);
            eventArgs.Parameter = _journalVM._CREC_ID;
        }
        private void AfterPredef_RecurringEntry(R_AfterOpenPredefinedDockEventArgs eventArgs)
        { }
        private void Predef_ActualJournalList(R_InstantiateDockEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(ActualJournalList);
            eventArgs.Parameter = "ACTUAL JOURNAL LIST";
        }
        private void AfterPredef_ActualJournalList(R_AfterOpenPredefinedDockEventArgs eventArgs)
        { }
        #endregion

        #region Search
        public async Task SearchAllAsync()
        {
            var loEx = new R_Exception();
            try
            {
                await _gridJournal.R_RefreshGrid(null);
                if (_journalVM.JournalList.Count == 0)
                {
                    R_MessageBox.Show("Warning", "No data found!", R_eMessageBoxButtonType.OK);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);

        }
        public async Task SearchWithFilterAsync()
        {
            var loEx = new R_Exception();
            try
            {
                await _gridJournal.R_RefreshGrid(true);
                if (_journalVM.JournalList.Count == 0)
                {
                    _journalVM.JournaDetailList.Clear();
                    R_MessageBox.Show("Warning", "No data found!", R_eMessageBoxButtonType.OK);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);

        }
        #endregion

        #region JournalGrid
        private async Task JournalGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var param = eventArgs.Parameter;
                await _journalVM.ShowAllJournals();
                eventArgs.ListEntityResult = _journalVM.JournalList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task JournalGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<JournalParamDTO>(eventArgs.Data);
                await _journalVM.GetJournal(loParam);
                eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalGrid_Display(R_DisplayEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (JournalGridDTO)eventArgs.Data;
                _journalVM._CREC_ID = loData.CREC_ID;
                _gridJournalDet.R_RefreshGrid(null);  
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion

        #region JournalDetailGrid
        private async Task JournalDetGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.GetJournalDetailList();
                eventArgs.ListEntityResult = _journalVM.JournaDetailList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion

        #region DepartmentLookup
        private async Task Dept_Before_Open_Lookup(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                eventArgs.Parameter = new GSL00700ParameterDTO();
                eventArgs.TargetPageType = typeof(GSL00700);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task Dept_After_Open_Lookup(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loTempResult = R_FrontUtility.ConvertObjectToObject<GSL00700DTO>(eventArgs.Result);
                _journalVM._SearchParam.CDEPT_CODE = loTempResult.CDEPT_CODE;
                _journalVM._SearchParam.CDEPT_NAME = loTempResult.CDEPT_NAME;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region RapidApprove
        private async Task RapidApproval_BeforeOpenPopup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            //if (_JournalListViewModel.JournalList.Count < 1)
            //{
            //    R_MessageBox.Show("", "You don’t have right to approve this journal type!", R_eMessageBoxButtonType.OK);
            //    goto EndBlock;
            //}
            //eventArgs.Parameter = _JournalListViewModel.JournalList;
            //eventArgs.TargetPageType = typeof(RapidApproveGLT00300);
        EndBlock:;
        }
        private async Task RapidApproval_AfterOpenPopup(R_AfterOpenPopupEventArgs eventArgs)
        {
        }
        #endregion
    }
}
