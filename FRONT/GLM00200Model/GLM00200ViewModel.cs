using GLM00200Common;
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
using System.Linq;
using System.Threading.Tasks;

namespace GLM00200Model
{
    public class GLM00200ViewModel : R_ViewModel<JournalDTO>
    {
        public GLM00200Model _model = new GLM00200Model();
        public PublicLookupModel _lookupModel = new PublicLookupModel();
        public ObservableCollection<JournalGridDTO> JournalList { get; set; } = new ObservableCollection<JournalGridDTO>();
        public ObservableCollection<JournalDetailGridDTO> JournaDetailList { get; set; } = new ObservableCollection<JournalDetailGridDTO>();
        public GSL00700DTO Dept { get; set; } = new GSL00700DTO();
        public JournalDTO Journal { get; set; } = new JournalDTO() {
        CDEPT_CODE= "CDEPT_CODE",
        CDEPT_NAME="CDEPT_NAME",
        CREF_NO= "CREF_NO",
        CREF_DATE= "CREF_DATE",
        CDOC_NO="CDOC_NO",
        CDOC_DATE="CDOC_DATE",
        IFREQUENCY=0,
        IAPPLIED=0,
        IPERIOD=0,
        CSTART_DATE=DateTime.Now.ToString("yyyyMMdd"),
        CNEXT_DATE= DateTime.Now.AddDays(1).ToString("yyyyMMdd"),







        };
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
        public List<PeriodDTO> Periods = new List<PeriodDTO>{
            new PeriodDTO { CPERIOD_MM_CODE = "01", CPERIOD_MM_TEXT= "01" },
            new PeriodDTO { CPERIOD_MM_CODE = "02", CPERIOD_MM_TEXT = "02" },
            new PeriodDTO { CPERIOD_MM_CODE = "03", CPERIOD_MM_TEXT = "03" },
            new PeriodDTO { CPERIOD_MM_CODE = "04", CPERIOD_MM_TEXT = "04" },
            new PeriodDTO { CPERIOD_MM_CODE = "05", CPERIOD_MM_TEXT = "05" },
            new PeriodDTO { CPERIOD_MM_CODE = "06", CPERIOD_MM_TEXT = "06" },
            new PeriodDTO { CPERIOD_MM_CODE = "07", CPERIOD_MM_TEXT = "07" },
            new PeriodDTO { CPERIOD_MM_CODE = "08", CPERIOD_MM_TEXT = "08" },
            new PeriodDTO { CPERIOD_MM_CODE = "09", CPERIOD_MM_TEXT = "09" },
            new PeriodDTO { CPERIOD_MM_CODE = "10", CPERIOD_MM_TEXT = "10" },
            new PeriodDTO { CPERIOD_MM_CODE = "11", CPERIOD_MM_TEXT = "11" },
            new PeriodDTO { CPERIOD_MM_CODE = "12", CPERIOD_MM_TEXT = "12" },
        };
        public DateTime _DREF_DATE { get; set; } = DateTime.Now;
        public string _CREC_ID { get; set; } = "";

        #region CRUD Journal
        public async Task GetJournal(JournalDTO loParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _model.R_ServiceGetRecordAsync(loParam);
                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveJournal(JournalDTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Init
        public async Task ShowAllJournals()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = new List<JournalGridDTO>();
                _SearchParam.CSTATUS = "";
                _SearchParam.CSEARCH_TEXT = "";
                _SearchParam.CTRANS_CODE = "000030";
                _SearchParam.LSHOW_ALL = true;
                R_FrontContext.R_SetContext(RecurringJournalContext.OSEARCH_PARAM, _SearchParam);
                loResult = await _model.GetAllRecurringListAsync();
                JournalList = new ObservableCollection<JournalGridDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetJournalDetailList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = new List<JournalDetailGridDTO>();
                R_FrontContext.R_SetContext(RecurringJournalContext.CREC_ID, _CREC_ID);
                loResult = await _model.GetAllJournalDetailListAsync();
                JournaDetailList = new ObservableCollection<JournalDetailGridDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetFirstDepartData()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _lookupModel.GSL00700GetDepartmentListAsync(new GSL00700ParameterDTO());
                Dept = loResult.Data.FirstOrDefault();
                _SearchParam.CDEPT_CODE = Dept.CDEPT_CODE;
                _SearchParam.CDEPT_NAME = Dept.CDEPT_NAME;
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
    }
}
