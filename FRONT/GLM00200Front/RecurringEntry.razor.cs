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
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLM00200Front
{
    public partial class RecurringEntry : R_Page
    {
        private GLM00200ViewModel _journalVM = new GLM00200ViewModel();
        private R_Grid<JournalDetailGridDTO> _gridJournalDet;
        private R_ConductorGrid _conJournalDet;
        private R_Conductor _conJournalNavigator;
        public string Title { get; set; }
        public DateTime DREF_DATE = DateTime.Now;
        public DateTime DDOC_DATE = DateTime.Now;
        public DateTime DSTART_DATE = DateTime.Now;
        public DateTime DNEXT_DATE = DateTime.Now;

        #region Form Enable/Disable
        public bool ENABLE_CACTION = true;
        public bool ENABLE_CCOMPANY_ID = true;
        public bool ENABLE_CDEPT_CODE = true;
        public bool ENABLE_CTRANS_CODE = true;
        public bool ENABLE_CREF_NO = true;
        public bool ENABLE_CDOC_NO = true;
        public bool ENABLE_CDOC_DATE = true;
        public bool ENABLE_IFREQUENCY = true;
        public bool ENABLE_IAPPLIED = true;
        public bool ENABLE_IPERIOD = true;
        public bool ENABLE_CSTART_DATE = true;
        public bool ENABLE_CNEXT_DATE = true;
        public bool ENABLE_CLAST_DATE = true;
        public bool ENABLE_CTRANS_DESC = true;
        public bool ENABLE_CCURRENCY_CODE = true;
        public bool ENABLE_LFIX_RATE = true;
        public bool ENABLE_NLBASE_RATE = true;
        public bool ENABLE_NLCURRENCY_RATE = true;
        public bool ENABLE_NBBASE_RATE = true;
        public bool ENABLE_NBCURRENCY_RATE = true;
        public bool ENABLE_NPRELIST_AMOUNT = true;
        public bool ENABLE_NNTRANS_AMOUNT_C = true;
        public bool ENABLE_NNTRANS_AMOUNT_D = true;
        public bool ENABLE_CUPDATE_BY = true;
        public bool ENABLE_DUPDATE_DATE = true;
        public bool ENABLE_CCREATE_BY = true;
        public bool ENABLE_DCREATE_DATE = true;
        public bool ENABLE_CLANGUAGE_ID = true;
        #endregion
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                _journalVM._CREC_ID = (string)poParameter;
                await _journalVM.GetVAR_GSM_COMPANY_DTOAsync();
                await _journalVM.GetCurrenciesAsync();
                await _journalVM.GetJournal(new JournalDTO() { CJRN_ID=_journalVM._CREC_ID});
                _gridJournalDet.R_RefreshGrid(null);
                _gridJournalDet.AutoFitAllColumnsAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region AddJournal
        private async Task BeforeAddJournal(R_BeforeAddEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {

            }
            catch (Exception ex)
            {
                loEx.ThrowExceptionIfErrors();
            }
        }
        private async Task JournalForm_Validation(R_ValidationEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            if (loEx.HasError)
                eventArgs.Cancel = true;
            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalForm_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task JournalForm_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.SaveJournal((JournalDTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Form Control
        private void DSTART_DATE_ONCHANGED()
        {
            var loEx = new R_Exception();
            try
            {
                DNEXT_DATE = DSTART_DATE.AddDays(1);
                _journalVM.Journal.CNEXT_DATE = DNEXT_DATE.ToString("yyMMdd");
                //_journalVM.
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private void LFIX_RATE_ONCHANGED()
        {
            var loEx = new R_Exception();
            try
            {
                if (_journalVM.Journal.LFIX_RATE)
                {
                    ENABLE_NLBASE_RATE = false;
                    ENABLE_NBBASE_RATE = false;
                    ENABLE_NLCURRENCY_RATE = false;
                    ENABLE_NBCURRENCY_RATE = false;
                }
                else
                {
                    ENABLE_NLBASE_RATE = true;
                    ENABLE_NBBASE_RATE = true;
                    ENABLE_NLCURRENCY_RATE = true;
                    ENABLE_NBCURRENCY_RATE = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private void NLBASE_RATE_VALUE()
        {
            var loEx = new R_Exception();
            try
            {
                if (_journalVM.Journal.CCURRENCY_CODE != _journalVM._GSM_COMPANY.CLOCAL_CURRENCY_CODE && _journalVM.Journal.LFIX_RATE == true)
                {
                    ENABLE_NLBASE_RATE = true;
                    ENABLE_NLCURRENCY_RATE = true;
                }
                else
                {
                    ENABLE_NLBASE_RATE = false;
                    ENABLE_NLCURRENCY_RATE = false;
                }
                if (_journalVM.Journal.CCURRENCY_CODE != _journalVM._GSM_COMPANY.CBASE_CURRENCY_CODE && _journalVM.Journal.LFIX_RATE == true)
                {
                    ENABLE_NBBASE_RATE = true;
                    ENABLE_NBCURRENCY_RATE = true;
                }
                else
                {
                    ENABLE_NBBASE_RATE = false;
                    ENABLE_NBCURRENCY_RATE = false;
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
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
        private async Task JournalDetGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                //var loParam = R_FrontUtility.ConvertObjectToObject<JournalDTO>(eventArgs.Data);
                //await _journalVM.GetJournal(loParam);
                //eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalDetGrid_Display(R_DisplayEventArgs eventArgs)
        {

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
                _journalVM.Journal.CDEPT_CODE = loTempResult.CDEPT_CODE;
                _journalVM.Journal.CDEPT_NAME = loTempResult.CDEPT_NAME;
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
