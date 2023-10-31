using BlazorClientHelper;
using GLM00200Common;
using GLM00200Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace GLM00200Front
{
    public partial class RecurringEntry : R_Page
    {
        private GLM00200ViewModel _journalVM = new GLM00200ViewModel();
        private R_Grid<JournalDetailGridDTO> _gridJournalDet;
        private R_Conductor _conJournalNavigator;
        private R_ConductorGrid _conJournalDetail;
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
                    await _journalVM.GetInitData();
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
                var loData = (JournalDTO)eventArgs.Data;
                if (eventArgs.ConductorMode != R_eConductorMode.Normal)
                {
                    if (string.IsNullOrEmpty(loData.CREF_NO) || string.IsNullOrWhiteSpace(loData.CREF_NO))
                    {
                        loEx.Add("", "Account No. is required!");
                    }
                    if (_journalVM._defaultValue_DREF_DATE == DateTime.MinValue)
                    {
                        loEx.Add("", "Reference Date is required!");
                    }
                    if (_journalVM._defaultValue_DREF_DATE < DateTime.ParseExact(_journalVM._InitData.OCURRENT_PERIOD_START_DATE.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture))
                    {
                        loEx.Add("", "Reference Date cannot be before Current Period!");
                    }
                    if (_journalVM._defaultValue_DREF_DATE > _journalVM._defaultValue_DSTART_DATE)
                    {
                        loEx.Add("", "Reference Date cannot be after Start Date!");
                    }
                    if (_journalVM._defaultValue_DSTART_DATE < DateTime.ParseExact(_journalVM._InitData.OCURRENT_PERIOD_START_DATE.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture))
                    {
                        loEx.Add("", "Start Date cannot be before Current Period!");
                    }
                    if (string.IsNullOrEmpty(loData.CDOC_NO) || string.IsNullOrWhiteSpace(loData.CDOC_NO) && _journalVM._defaultValue_DDOC_DATE != DateTime.MinValue)
                    {
                        loEx.Add("", "Please input Document No.!");
                    }
                    if (_journalVM._defaultValue_DDOC_DATE == DateTime.MinValue && _journalVM._defaultValue_DDOC_DATE > DateTime.Now)
                    {
                        loEx.Add("", "Document Date cannot be after today");
                    }
                    if (_journalVM._defaultValue_DDOC_DATE == DateTime.MinValue && _journalVM._defaultValue_DDOC_DATE < DateTime.ParseExact(_journalVM._InitData.OCURRENT_PERIOD_START_DATE.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture))
                    {
                        loEx.Add("", "Document Date cannot be before Current Period!");
                    }
                    if (!string.IsNullOrEmpty(loData.CDOC_NO) || !string.IsNullOrWhiteSpace(loData.CDOC_NO) && _journalVM._defaultValue_DDOC_DATE == DateTime.MinValue)
                    {
                        loEx.Add("", "Please input Document Date!");
                    }
                    if (_journalVM._defaultValue_DDOC_DATE > _journalVM._defaultValue_DSTART_DATE)
                    {
                        loEx.Add("", "Document Date cannot be after Start Date!");
                    }
                    if (string.IsNullOrEmpty(loData.CTRANS_DESC) || string.IsNullOrWhiteSpace(loData.CTRANS_DESC))
                    {
                        loEx.Add("", "Description is required!");
                    }

                    if (loData.NPRELIST_AMOUNT > 0 && loData.NPRELIST_AMOUNT != loData.NNTRANS_AMOUNT_D)
                    {
                        loEx.Add("", "Journal amount is not equal to Prelist!");
                    }

                    if (loData.NLBASE_RATE <= 0)
                    {
                        loEx.Add("", "Local Currency Base Rate must be greater than 0!");
                    }

                    if (loData.NLCURRENCY_RATE <= 0)
                    {
                        loEx.Add("", "Local Currency Rate must be greater than 0!");
                    }

                    if (loData.NBBASE_RATE <= 0)
                    {
                        loEx.Add("", "Base Currency Base Rate must be greater than 0!");
                    }

                    if (loData.NBCURRENCY_RATE <= 0)
                    {
                        loEx.Add("", "Base Currency Rate must be greater than 0!");
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
                var loData = (JournalDTO)eventArgs.Data;
                await _journalVM.GetCurrencies();//get list currencies
                await _journalVM.GetInitData();//get company data
                await _journalVM.GetListCenter(); // get center list for add/edit detail(grid) data

                _enableCrudJournalDetail = true; //enable grid to add/edit/delete
                _journalVM._JournaDetailListTemp = _journalVM._JournaDetailList; //add recent 
                _journalVM._JournaDetailList = new();

                loData.CCURRENCY_CODE = _journalVM._InitData.OGSM_COMPANY.CLOCAL_CURRENCY_CODE;//set default ccurrency data when addmode
                eventArgs.Data = loData;    //return
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
                if (loTempResult == null)
                {
                    return;
                }
                _journalVM.Data.CDEPT_CODE = loTempResult.CDEPT_CODE;
                _journalVM.Data.CDEPT_NAME = loTempResult.CDEPT_NAME;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region JournalDetailGrid
        private void JurnalDetail_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }
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
                CPROGRAM_CODE = "GLM00200",
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
        private void JurnalDetail_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (JournalDetailGridDTO)eventArgs.Data;
                if (eventArgs.ConductorMode != R_eConductorMode.Normal)
                {
                    if (string.IsNullOrWhiteSpace(loData.CGLACCOUNT_NO))
                    {
                        loEx.Add("", "Account No. is required!");
                    }

                    if (string.IsNullOrWhiteSpace(loData.CCENTER_CODE) && (loData.CBSIS == "B" && _journalVM._InitData.OGSM_COMPANY.LENABLE_CENTER_BS == true) || (loData.CBSIS == "I" && _journalVM._InitData.OGSM_COMPANY.LENABLE_CENTER_IS == true))
                    {
                        loEx.Add("", $"Center Code is required for Account No. {loData.CGLACCOUNT_NO}!");
                    }

                    if (loData.NDEBIT == 0 && loData.NCREDIT == 0)
                    {
                        loEx.Add("", "Journal amount cannot be 0!");
                    }

                    if (loData.NDEBIT > 0 && loData.NCREDIT > 0)
                    {
                        loEx.Add("", "Journal amount can only be either Debit or Credit!");
                    }

                    if (eventArgs.ConductorMode == R_eConductorMode.Add)
                    {
                        if (_journalVM._JournaDetailList.Any(item => item.CGLACCOUNT_NO == loData.CGLACCOUNT_NO))
                        {
                            loEx.Add("", $"Account No. {loData.CGLACCOUNT_NO} already exists!");
                        }
                    }

                    if (_journalVM._JournaDetailList.Count(item => item.CGLACCOUNT_NO == loData.CGLACCOUNT_NO) > 1 ||
                        (_journalVM._JournaDetailList.Any(item => item.CGLACCOUNT_NO == loData.CGLACCOUNT_NO) && eventArgs.ConductorMode == R_BlazorFrontEnd.Enums.R_eConductorMode.Edit))
                    {
                        loEx.Add("", $"Account No. {loData.CGLACCOUNT_NO} already exists!");
                    }

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private void JurnalDetail_Display(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (JournalDetailGridDTO)eventArgs.Data;

                //findout CREDIT OR DEBIT
                loData.CDBCR = loData.NDEBIT > 0 ? "D" : "C";

                //fill ccentercode if null based on ccentername
                if (string.IsNullOrEmpty(loData.CCENTER_CODE) || string.IsNullOrWhiteSpace(loData.CCENTER_CODE))
                {
                    foreach (var loItem in _journalVM._ListCenter)
                    {
                        if (loData.CCENTER_NAME == loItem.CCENTER_NAME)
                            loData.CCENTER_CODE = loItem.CCENTER_CODE;
                    }
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private void JurnalDetail_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (JournalDetailGridDTO)eventArgs.Data;

            //create increment data grid
            if (_journalVM._JournaDetailList.Any())
            {
                // Find the maximum INO value in the list and increment it by 1
                int maxINO = _journalVM._JournaDetailList.Max(item => item.INO);
                loData.INO = maxINO + 1;
            }
            else
            {
                // If the list is empty, set INO to 1 (or another initial value)
                loData.INO = 1;
            }


            //assign to data grid
            eventArgs.Data = loData;
        }
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
                _journalVM._defaultValue_DNEXT_DATE = _journalVM._defaultValue_DSTART_DATE.AddDays(1);
                _journalVM.Data.CNEXT_DATE = _journalVM._defaultValue_DNEXT_DATE.ToString("yyMMdd");
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
                if (_journalVM.Data.LFIX_RATE)
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
                if (_journalVM.Data.CCURRENCY_CODE != _journalVM._InitData.OGSM_COMPANY.CLOCAL_CURRENCY_CODE && _journalVM.Data.LFIX_RATE == true)
                {
                    _enable_NLBASE_RATE = true;
                    _enable_NLCURRENCY_RATE = true;
                }
                else
                {
                    _enable_NLBASE_RATE = false;
                    _enable_NLCURRENCY_RATE = false;
                }
                if (_journalVM._Journal.CCURRENCY_CODE != _journalVM._InitData.OGSM_COMPANY.CBASE_CURRENCY_CODE && _journalVM.Data.LFIX_RATE == true)
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
