using GLM00200Common;
using GLM00200Common.Init_DTO_s;
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
        public GLM00200Model _model = new GLM00200Model();
        public PublicLookupModel _lookupModel = new PublicLookupModel();

        public RecurringJournalListParamDTO _SearchParam { get; set; } = new RecurringJournalListParamDTO();
        public ObservableCollection<JournalGridDTO> _JournalList { get; set; } = new ObservableCollection<JournalGridDTO>();
        public ObservableCollection<JournalDetailActualGridDTO> _ActualJournalList { get; set; } = new ObservableCollection<JournalDetailActualGridDTO>();
        public ObservableCollection<JournalDetailGridDTO> _JournaDetailList { get; set; } = new ObservableCollection<JournalDetailGridDTO>();
        public ObservableCollection<JournalDetailGridDTO> _JournaDetailListTemp { get; set; } = new ObservableCollection<JournalDetailGridDTO>();
        public GSL00700DTO _Dept { get; set; } = new GSL00700DTO();
        public JournalParamDTO _Journal { get; set; } = new JournalParamDTO();
        public List<GSL00900DTO> _ListCenter { get; set; } = new List<GSL00900DTO>();
        public List<StatusDTO> _StatusList { get; set; } = new List<StatusDTO>();
        public List<CurrencyDTO> _CurrencyList { get; set; } = new List<CurrencyDTO>();
        public List<PeriodFrontDTO> _Periods = new List<PeriodFrontDTO>();
        public InitDTO _InitData { get; set; } = new InitDTO();
        public CurrencyRateResult _CURRENCY_RATE_RESULT = new CurrencyRateResult();
        public DateTime _DREF_DATE { get; set; } = DateTime.Now;
        public DateTime _DDOC_DATE { get; set; } = DateTime.Now;
        public DateTime _DSTART_DATE { get; set; } = DateTime.Now;
        public string _CREC_ID { get; set; } = "";

        public DateTime _defaultValue_DREF_DATE = DateTime.Now;
        public DateTime _defaultValue_DDOC_DATE = DateTime.Now;
        public DateTime _defaultValue_DSTART_DATE = DateTime.Now;
        public DateTime _defaultValue_DNEXT_DATE = DateTime.Now;

        private const string CTRANS_CODE = "000030";

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

        #region JournalDetails
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
        #endregion

        #region Init
        public async Task GetFirstDepartData()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _lookupModel.GSL00700GetDepartmentListAsync();
                _Dept = loResult.FirstOrDefault();
                _SearchParam.CDEPT_CODE = _Dept.CDEPT_CODE;
                _SearchParam.CDEPT_NAME = _Dept.CDEPT_NAME;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetInitData()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = await _model.GetInitDataAsync();
                _InitData = loData.data;
                _DREF_DATE = _InitData.DTODAY;
                _DSTART_DATE = _DREF_DATE = _InitData.DTODAY;
                _DDOC_DATE = _InitData.DTODAY;
                _Periods = Enumerable.Range(1, 12).Select(month => new PeriodFrontDTO
                {
                    CPERIOD_MM_CODE = month.ToString("D2"),
                    CPERIOD_MM_TEXT = month.ToString("D2")
                }).ToList();
                _SearchParam.CPERIOD_MM = _Periods.FirstOrDefault().CPERIOD_MM_CODE;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetStatusList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var rtnTemp = await _model.GetStatusListAsync();
                _StatusList = rtnTemp;
                _SearchParam.CSTATUS = _StatusList.FirstOrDefault().CCODE;
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
                var loResult = await _lookupModel.GSL00900GetCenterListAsync();
                _ListCenter = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetCurrencies()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loRtn = await _model.GetCurrencyListAsync();
                _CurrencyList = loRtn;
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
                R_FrontContext.R_SetContext(RecurringJournalContext.CRATETYPE_CODE, _InitData.SYSTEM_PARAM.CRATETYPE_CODE);
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
                bool llAutoCommit = _InitData.SYSTEM_PARAM.LCOMMIT_APVJRN;
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
