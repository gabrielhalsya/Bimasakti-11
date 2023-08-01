using GLM00200Common;
using GLM00200Common.DTO_s;
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

        public bool _enableCrudJournalDetail = false;
        public bool _enableAddJournalDetail = false;
        public bool _enableEditJournalDetail = false;
        public bool _enableDeleteJournalDetail = false;

        #region Form Enable/Disable
        public bool ENABLE_NLBASE_RATE = false;
        public bool ENABLE_NLCURRENCY_RATE = false;
        public bool ENABLE_NBBASE_RATE = false;
        public bool ENABLE_NBCURRENCY_RATE = false;
        #endregion
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                _journalVM._CREC_ID = (string)poParameter;
                //var loParam = R_FrontUtility.ConvertObjectToObject<R_ServiceGetRecordEventArgs>(poParameter);
                await _journalVM.GetVAR_GSM_COMPANY_DTOAsync();
                await _journalVM.GetVAR_GL_SYSTEM_PARAMAsync();
                await _journalVM.GetCurrenciesAsync();
                _conJournalNavigator.R_GetEntity(_journalVM._CREC_ID);
                await _gridJournalDet.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region JournalForm
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
        private async Task JournalForm_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<JournalParamDTO>(eventArgs.Data);
                loParam.ListJournalDetail = new List<JournalDetailGridDTO>(_journalVM.JournaDetailList);
                await _journalVM.SaveJournal(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalForm_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.GetJournal(new JournalParamDTO() { CREC_ID = R_FrontUtility.ConvertObjectToObject<string>(eventArgs.Data) });
                eventArgs.Result = _journalVM.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        //private async Task JournalForm_AfterAdd(R_AfterAddEventArgs eventArgs)
        //{
        //    var loEx = new R_Exception();
        //    try
        //    {
        //        _enableCrudJournalDetail = true;
        //        _journalVM.JournaDetailListTemp = _journalVM.JournaDetailList;
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }
        //    loEx.ThrowExceptionIfErrors();
        //}
        #endregion

        #region Form Control
        private async Task DSTART_DATE_ONCHANGED()
        {
            var loEx = new R_Exception();
            try
            {
                DNEXT_DATE = DSTART_DATE.AddDays(1);
                _journalVM.Journal.CNEXT_DATE = DNEXT_DATE.ToString("yyMMdd");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task OnChanged_LFIX_RATE()
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
        private async Task OnChanged_CurrencyCode()
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.RefreshCurrencyRate();
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
