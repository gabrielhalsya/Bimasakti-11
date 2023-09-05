using BlazorClientHelper;
using GLM00200Common;
using GLM00200Common.DTO_s;
using GLM00200Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace GLM00200Front
{
    public partial class RecurringEntry : R_Page
    {
        private GLM00200ViewModel _journalVM = new GLM00200ViewModel();
        private R_Grid<JournalDetailGridDTO> _gridJournalDet;
        private R_ConductorGrid _conJournalDet;
        private R_Conductor _conJournalNavigator;
        private R_ConductorGrid _conJournalDetail;
        private string _Title { get; set; }
        private DateTime _defaultValue_DREF_DATE = DateTime.Now;
        private DateTime _defaultValue_DDOC_DATE = DateTime.Now;
        private DateTime _defaultValue_DSTART_DATE = DateTime.Now;
        private DateTime _defaultValue_DNEXT_DATE = DateTime.Now;
        private bool _enableCrudJournalDetail = false;
        [Inject] IClientHelper _clientHelper { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                string pcParam = poParameter.ToString();
                if (!string.IsNullOrWhiteSpace(pcParam))
                {
                    _journalVM._CREC_ID = pcParam;
                    await _journalVM.GetVAR_GSM_COMPANY_DTOAsync();
                    await _journalVM.GetVAR_GL_SYSTEM_PARAMAsync();
                    await _journalVM.GetCurrenciesAsync();
                    await _conJournalNavigator.R_GetEntity(_journalVM._CREC_ID);
                    await _gridJournalDet.R_RefreshGrid(null);
                }
                //var loParam = R_FrontUtility.ConvertObjectToObject<R_ServiceGetRecordEventArgs>(poParameter);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region JournalForm
        private void JournalForm_Validation(R_ValidationEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (JournalParamDTO)eventArgs.Data;

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
                loParam.ListJournalDetail = new List<JournalDetailGridDTO>(_journalVM._JournaDetailList);
                await _journalVM.SaveJournal(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _journalVM._Journal;
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
                JournalParamDTO loParam = default;
                if (eventArgs.Data.GetType() == typeof(string))
                {
                    loParam = new JournalParamDTO()
                    {
                        CREC_ID = R_FrontUtility.ConvertObjectToObject<string>(eventArgs.Data)
                    };
                }
                else
                {
                    loParam = R_FrontUtility.ConvertObjectToObject<JournalParamDTO>(eventArgs.Data);
                }
                //await _journalVM.GetJournal(new JournalParamDTO() { CREC_ID = R_FrontUtility.ConvertObjectToObject<string>(eventArgs.Data) });
                await _journalVM.GetJournal(loParam);
                eventArgs.Result = _journalVM._Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task JournalForm_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                _enableCrudJournalDetail = true; //enable grid to add/edit/delete
                _journalVM._JournaDetailListTemp = _journalVM._JournaDetailList; //add recent 
                _journalVM._JournaDetailList = new();
                await _journalVM.GetListCenter();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private void JournalForm_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
            _enableCrudJournalDetail = false;

            if (eventArgs.ConductorMode == R_BlazorFrontEnd.Enums.R_eConductorMode.Add)
            { _journalVM._JournaDetailList = _journalVM._JournaDetailListTemp; }
        }
        private void JournalForm_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            _enableCrudJournalDetail = true;
        }
        private void JurnalDetail_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
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
                _journalVM._Journal.CDEPT_CODE = loTempResult.CDEPT_CODE;
                _journalVM._Journal.CDEPT_NAME = loTempResult.CDEPT_NAME;
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
                eventArgs.ListEntityResult = _journalVM._JournaDetailList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private void JournalDetailBeforeOpenLookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {
            var param = new GSL00500ParameterDTO
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROGRAM_CODE = "GLM00100",
                CUSER_ID = _clientHelper.UserId,
                CUSER_LANGUAGE = _clientHelper.CultureUI.TwoLetterISOLanguageName,
                CBSIS = "",
                CDBCR = "",
                CCENTER_CODE = "",
                CPROPERTY_ID = "",
                LCENTER_RESTR = false,
                LUSER_RESTR = false
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSL00500);
        }
        private void JournalDetailAfterOpenLookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            var loTempResult = (GSL00500DTO)eventArgs.Result;
            if (loTempResult == null)
                return;
            var loGetData = (JournalDetailGridDTO)eventArgs.ColumnData;
            loGetData.CGLACCOUNT_NO = loTempResult.CGLACCOUNT_NO;
            loGetData.CGLACCOUNT_NAME = loTempResult.CGLACCOUNT_NAME;
            loGetData.CBSIS = loTempResult.CBSIS;
            //loGetData.CBSIS = loTempResult.CBSIS_DESCR.Contains("B") ? 'B' : (loTempResult.CBSIS_DESCR.Contains("I") ? 'I' :default(char));
        }
        private void JurnalDetail_Display(R_DisplayEventArgs eventArgs) { }
        private void JurnalDetail_AfterAdd(R_AfterAddEventArgs eventArgs) { }
        private void JurnalDetail_Validation(R_ValidationEventArgs eventArgs) { }
        #endregion

        #region Form Control
        private bool _enable_NLBASE_RATE = false;
        private bool _enable_NLCURRENCY_RATE = false;
        private bool _enable_NBBASE_RATE = false;
        private bool _enable_NBCURRENCY_RATE = false;
        private void OnChangedStartDate()
        {
            var loEx = new R_Exception();
            try
            {
                _defaultValue_DNEXT_DATE = _defaultValue_DSTART_DATE.AddDays(1);
                _journalVM._Journal.CNEXT_DATE = _defaultValue_DNEXT_DATE.ToString("yyMMdd");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private void OnChanged_LFIX_RATE()
        {
            var loEx = new R_Exception();
            try
            {
                if (_journalVM._Journal.LFIX_RATE)
                {
                    _enable_NLBASE_RATE = false;
                    _enable_NBBASE_RATE = false;
                    _enable_NLCURRENCY_RATE = false;
                    _enable_NBCURRENCY_RATE = false;
                }
                else
                {
                    _enable_NLBASE_RATE = true;
                    _enable_NBBASE_RATE = true;
                    _enable_NLCURRENCY_RATE = true;
                    _enable_NBCURRENCY_RATE = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task OnChanged_CurrencyCodeAsync()
        {
            var loEx = new R_Exception();
            try
            {
                await _journalVM.RefreshCurrencyRate();
                if (_journalVM._Journal.CCURRENCY_CODE != _journalVM._GSM_COMPANY.CLOCAL_CURRENCY_CODE && _journalVM._Journal.LFIX_RATE == true)
                {
                    _enable_NLBASE_RATE = true;
                    _enable_NLCURRENCY_RATE = true;
                }
                else
                {
                    _enable_NLBASE_RATE = false;
                    _enable_NLCURRENCY_RATE = false;
                }
                if (_journalVM._Journal.CCURRENCY_CODE != _journalVM._GSM_COMPANY.CBASE_CURRENCY_CODE && _journalVM._Journal.LFIX_RATE == true)
                {
                    _enable_NBBASE_RATE = true;
                    _enable_NBCURRENCY_RATE = true;
                }
                else
                {
                    _enable_NBBASE_RATE = false;
                    _enable_NBCURRENCY_RATE = false;
                }

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
