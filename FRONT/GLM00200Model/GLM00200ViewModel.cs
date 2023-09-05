﻿using GLM00200Common;
using GLM00200Common.DTO_s;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSModel;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GLM00200Model
{
    public class GLM00200ViewModel : R_ViewModel<JournalParamDTO>
    {
        private const string CTRANS_CODE = "000030";

        public GLM00200Model _model = new GLM00200Model();
        public PublicLookupModel _lookupModel = new PublicLookupModel();

        public ObservableCollection<JournalGridDTO> _JournalList { get; set; } = new ObservableCollection<JournalGridDTO>();
        public ObservableCollection<JournalDetailActualGridDTO> _ActualJournalList { get; set; } = new ObservableCollection<JournalDetailActualGridDTO>();
        public ObservableCollection<JournalDetailGridDTO> _JournaDetailList { get; set; } = new ObservableCollection<JournalDetailGridDTO>();
        public ObservableCollection<JournalDetailGridDTO> _JournaDetailListTemp { get; set; } = new ObservableCollection<JournalDetailGridDTO>();
        public GSL00700DTO _Dept { get; set; } = new GSL00700DTO();
        public List<GSL00900DTO> _ListCenter { get; set; } = new List<GSL00900DTO>();
        public JournalParamDTO _Journal { get; set; } = new JournalParamDTO();
        public RecurringJournalListParamDTO _SearchParam { get; set; } = new RecurringJournalListParamDTO();
        public VAR_CCURRENT_PERIOD_START_DATE_DTO _CCURRENT_PERIOD_START_DATE { get; set; } = new VAR_CCURRENT_PERIOD_START_DATE_DTO();
        public VAR_CSOFT_PERIOD_START_DATE_DTO _CSOFT_PERIOD_START_DATE { get; set; } = new VAR_CSOFT_PERIOD_START_DATE_DTO();
        public VAR_GL_SYSTEM_PARAM_DTO _GL_SYSTEM_PARAM { get; set; } = new VAR_GL_SYSTEM_PARAM_DTO();
        public VAR_GSM_COMPANY_DTO _GSM_COMPANY { get; set; } = new VAR_GSM_COMPANY_DTO();
        public VAR_GSM_PERIOD_DTO _GSM_PERIOD { get; set; } = new VAR_GSM_PERIOD_DTO();
        public VAR_GSM_TRANSACTION_CODE_DTO _GSM_TRANSACTION_CODE { get; set; } = new VAR_GSM_TRANSACTION_CODE_DTO();
        public VAR_IUNDO_COMMIT_JRN_DTO _IUNDO_COMMIT_JRN { get; set; } = new VAR_IUNDO_COMMIT_JRN_DTO();
        public List<VAR_STATUS_DTO> _STATUS_LIST { get; set; } = new List<VAR_STATUS_DTO>();
        public List<VAR_CURRENCY> _CURRENCY_LIST { get; set; } = new List<VAR_CURRENCY>();
        public List<PeriodDTO> Periods = Enumerable.Range(1, 12).Select(month => new PeriodDTO { CPERIOD_MM_CODE = month.ToString("D2"), CPERIOD_MM_TEXT = month.ToString("D2") }).ToList();
        public REFRESH_CURRENCY_RATE_RESULT _CURRENCY_RATE_RESULT = new REFRESH_CURRENCY_RATE_RESULT();
        public DateTime _DREF_DATE { get; set; } = DateTime.Now;
        public DateTime _DSTART_DATE { get; set; } = DateTime.Now;
        public string _CREC_ID { get; set; } = "";

        #region CRUD Journal
        public async Task ShowAllJournals()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = new List<JournalGridDTO>();
                _SearchParam.CSTATUS = "";
                _SearchParam.CSEARCH_TEXT = "";
                _SearchParam.CTRANS_CODE = CTRANS_CODE;
                _SearchParam.LSHOW_ALL = true;
                R_FrontContext.R_SetContext(RecurringJournalContext.OSEARCH_PARAM, _SearchParam);
                loResult = await _model.GetAllRecurringListAsync();
                _JournalList = new ObservableCollection<JournalGridDTO>(loResult);
                foreach (var loJournal in _JournalList)
                {
                    if (loJournal.CSTART_DATE != "" || loJournal.CSTART_DATE != null)
                    {
                        loJournal.DSTART_DATE = DateTime.ParseExact(loJournal.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    if (loJournal.CNEXT_DATE != "" || loJournal.CNEXT_DATE != null)
                    {
                        loJournal.DNEXT_DATE = DateTime.ParseExact(loJournal.CNEXT_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetJournal(JournalParamDTO loParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _model.R_ServiceGetRecordAsync(loParam);
                _Journal = R_FrontUtility.ConvertObjectToObject<JournalParamDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveJournal(JournalParamDTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
            try
            {
                poNewEntity.CTRANS_CODE = CTRANS_CODE;
                var loResult = await _model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                _Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        public async Task GetJournalDetailList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = new List<JournalDetailGridDTO>();
                R_FrontContext.R_SetContext(RecurringJournalContext.CREC_ID, _CREC_ID);
                loResult = await _model.GetAllJournalDetailListAsync();
                loResult = loResult.Select((data, i) => { data.INO = i + 1; return data; }).ToList();
                _JournaDetailList = new ObservableCollection<JournalDetailGridDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetActualJournalDetailList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = new List<JournalDetailActualGridDTO>();
                R_FrontContext.R_SetContext(RecurringJournalContext.CREC_ID, _CREC_ID);
                loResult = await _model.GetAllJournalActualDetailListAsync();
                //loResult = loResult.Select((data, i) => { data.INO = i + 1; return data; }).ToList();
                _ActualJournalList = new ObservableCollection<JournalDetailActualGridDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #region Init
        public async Task GetFirstDepartData()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _lookupModel.GSL00700GetDepartmentListAsync(new GSL00700ParameterDTO());
                _Dept = loResult.Data.FirstOrDefault();
                _SearchParam.CDEPT_CODE = _Dept.CDEPT_CODE;
                _SearchParam.CDEPT_NAME = _Dept.CDEPT_NAME;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetListCenter()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _lookupModel.GSL00900GetCenterListAsync(new GSL00900ParameterDTO());
                _ListCenter = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetVAR_GSM_COMPANY_DTOAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetVAR_GSM_COMPANY_DTOAsync();
                _GSM_COMPANY = rtnTemp;
                _Journal.CCURRENCY_CODE = _GSM_COMPANY.CLOCAL_CURRENCY_CODE;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetVAR_GL_SYSTEM_PARAMAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetVAR_GL_SYSTEM_PARAMAsync();
                _GL_SYSTEM_PARAM = rtnTemp;
                _SearchParam.CPERIOD_MM = _GL_SYSTEM_PARAM.CSOFT_PERIOD_MM;
                _SearchParam.CPERIOD_YYYY = int.Parse(_GL_SYSTEM_PARAM.CSOFT_PERIOD_YY);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetCCURRENT_PERIOD_START_DATEAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetContext(RecurringJournalContext.CCURRENT_PERIOD_YY, _GL_SYSTEM_PARAM.CCURRENT_PERIOD_YY);
                R_FrontContext.R_SetContext(RecurringJournalContext.CCURRENT_PERIOD_MM, _GL_SYSTEM_PARAM.CCURRENT_PERIOD_MM);
                var rtnTemp = await _model.GetCCURRENT_PERIOD_START_DATEAsync();
                _CCURRENT_PERIOD_START_DATE = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetCSOFT_PERIOD_START_DATEAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetContext(RecurringJournalContext.CSOFT_PERIOD_MM, _GL_SYSTEM_PARAM.CSOFT_PERIOD_MM);
                R_FrontContext.R_SetContext(RecurringJournalContext.CSOFT_PERIOD_YY, _GL_SYSTEM_PARAM.CSOFT_PERIOD_YY);
                var rtnTemp = await _model.GetCSOFT_PERIOD_START_DATEAsync();
                _CSOFT_PERIOD_START_DATE = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetGSM_PERIODAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetGSM_PERIODAsync();
                _GSM_PERIOD = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetGSM_TRANSACTION_CODEAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetGSM_TRANSACTION_CODEAsync();
                _GSM_TRANSACTION_CODE = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetIUNDO_COMMIT_JRNAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetIUNDO_COMMIT_JRNAsync();
                _IUNDO_COMMIT_JRN = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetSTATUS_DTOAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetSTATUS_DTOAsync();
                _STATUS_LIST = rtnTemp;
                _SearchParam.CSTATUS = _STATUS_LIST.FirstOrDefault().CCODE;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetCurrenciesAsync()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetVAR_CURRENCIESAsync();
                _CURRENCY_LIST = rtnTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region RefreshCurrencyRate
        public async Task RefreshCurrencyRate()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var lcSTART_DATE = _DSTART_DATE.ToString("yyyyMMdd");
                R_FrontContext.R_SetContext(RecurringJournalContext.CCURRENCY_CODE, _Journal.CCURRENCY_CODE);
                R_FrontContext.R_SetContext(RecurringJournalContext.CRATETYPE_CODE, _GL_SYSTEM_PARAM.CRATETYPE_CODE);
                R_FrontContext.R_SetContext(RecurringJournalContext.CSTART_DATE, lcSTART_DATE);
                _CURRENCY_RATE_RESULT = await _model.RefreshCurrencyRateAsync();

                if (_CURRENCY_RATE_RESULT != null)
                {
                    _Journal.NLBASE_RATE = _CURRENCY_RATE_RESULT.NLBASE_RATE_AMOUNT;
                    _Journal.NLCURRENCY_RATE = _CURRENCY_RATE_RESULT.NLCURRENCY_RATE_AMOUNT;
                    _Journal.NBBASE_RATE = _CURRENCY_RATE_RESULT.NBBASE_RATE_AMOUNT;
                    _Journal.NBCURRENCY_RATE = _CURRENCY_RATE_RESULT.NBCURRENCY_RATE_AMOUNT;
                }
                else
                {
                    _Journal.NLBASE_RATE = 1;
                    _Journal.NLCURRENCY_RATE = 1;
                    _Journal.NBBASE_RATE = 1;
                    _Journal.NBCURRENCY_RATE = 1;
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Commit/Approval
        public async Task CommitJournal()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                string lcNewStatus = "";
                bool llAutoCommit = false;
                bool llUndoCommit = false;
                EModeCmmtAprvJRN loMode = EModeCmmtAprvJRN.Commit;
                if (_Journal.CSTATUS == "80")
                {
                    lcNewStatus = "20";
                    llUndoCommit = true;
                }
                else
                {
                    lcNewStatus = "80";
                    llUndoCommit = false;
                }
                R_FrontContext.R_SetContext(RecurringJournalContext.CJRN_ID_LIST, _CREC_ID);
                R_FrontContext.R_SetContext(RecurringJournalContext.CNEW_STATUS, lcNewStatus);
                R_FrontContext.R_SetContext(RecurringJournalContext.LAUTO_COMMIT, llAutoCommit);
                R_FrontContext.R_SetContext(RecurringJournalContext.LUNDO_COMMIT, llUndoCommit);
                R_FrontContext.R_SetContext(RecurringJournalContext.EMODE, loMode);

                await _model.JournalCommitApprovalAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task ApproveJournal()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                string lcNewStatus = "20";
                bool llAutoCommit = _GL_SYSTEM_PARAM.LCOMMIT_APVJRN;
                bool llUndoCommit = false;
                EModeCmmtAprvJRN loMode = EModeCmmtAprvJRN.Approval;

                R_FrontContext.R_SetContext(RecurringJournalContext.CJRN_ID_LIST, _CREC_ID);
                R_FrontContext.R_SetContext(RecurringJournalContext.CNEW_STATUS, lcNewStatus);
                R_FrontContext.R_SetContext(RecurringJournalContext.LAUTO_COMMIT, llAutoCommit);
                R_FrontContext.R_SetContext(RecurringJournalContext.LUNDO_COMMIT, llUndoCommit);
                R_FrontContext.R_SetContext(RecurringJournalContext.EMODE, loMode);

                await _model.JournalCommitApprovalAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region BatchJournalDetail

        #endregion


    }
}
