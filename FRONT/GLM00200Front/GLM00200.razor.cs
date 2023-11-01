using GLM00200Common;
using GLM00200Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

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
                await _journalVM.GetInitData();
                await _journalVM.GetStatusList();
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
                if (_journalVM._JournalList.Count == 0)
                {
                    await R_MessageBox.Show("Warning", "No data found!", R_eMessageBoxButtonType.OK);
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
                if (_journalVM._JournalList.Count == 0)
                {
                    _journalVM._JournaDetailList.Clear();
                    await R_MessageBox.Show("Warning", "No data found!", R_eMessageBoxButtonType.OK);
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
                eventArgs.ListEntityResult = _journalVM._JournalList;
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
                eventArgs.Result = _journalVM._Journal;
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
                await _gridJournalDet.R_RefreshGrid(null);
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
                eventArgs.ListEntityResult = _journalVM._JournaDetailList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion

        #region DepartmentLookup
        private void Dept_Before_Open_Lookup(R_BeforeOpenLookupEventArgs eventArgs)
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
        private void Dept_After_Open_Lookup(R_AfterOpenLookupEventArgs eventArgs)
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

    }
}
