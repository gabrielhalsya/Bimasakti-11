using CBT01100COMMON;
using CBT01100COMMON.DTO_s;
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

namespace CBT01100MODEL
{
    public class CBT01110ViewModel : R_ViewModel<CBT01111DTO>
    {
        #region Model
        private CBT01100InitModel _CBT01100InitModel = new CBT01100InitModel();
        private CBT01100Model _CBT01100Model = new CBT01100Model();
        private CBT01110Model _CBT01110Model = new CBT01110Model();
        #endregion

        #region Initial Data
        public CBT01100GSTransInfoDTO VAR_GSM_TRANSACTION_CODE { get; set; } = new CBT01100GSTransInfoDTO();
        public CBT01100GLSystemParamDTO VAR_GL_SYSTEM_PARAM { get; set; } = new CBT01100GLSystemParamDTO();
        public CBT01100CBSystemParamDTO VAR_CB_SYSTEM_PARAM { get; set; } = new CBT01100CBSystemParamDTO();
        public CBT01100GLSystemEnableOptionInfoDTO VAR_IUNDO_COMMIT_JRN { get; set; } = new CBT01100GLSystemEnableOptionInfoDTO();
        public CBT01100TodayDateDTO VAR_TODAY { get; set; } = new CBT01100TodayDateDTO();
        public CBT01100GSCompanyInfoDTO VAR_GSM_COMPANY { get; set; } = new CBT01100GSCompanyInfoDTO();
        public List<CBT01100GSCurrencyDTO> VAR_CURRENCY_LIST { get; set; } = new List<CBT01100GSCurrencyDTO>();
        public List<CBT01100GSCenterDTO> VAR_CENTER_LIST { get; set; } = new List<CBT01100GSCenterDTO>();
        public CBT01100GSPeriodDTInfoDTO VAR_CCURRENT_PERIOD_START_DATE { get; set; } = new CBT01100GSPeriodDTInfoDTO();
        public CBT01100GSPeriodYearRangeDTO VAR_GSM_PERIOD { get; set; }=new CBT01100GSPeriodYearRangeDTO();
        public List<CBT01100GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; } = new List<CBT01100GSGSBCodeDTO>();

        #endregion

        #region Public Property ViewModel
        public DateTime RefDate { get; set; }
        public DateTime? DocDate { get; set; }
        public CBT01110DTO Journal { get; set; } = new CBT01110DTO();
        public ObservableCollection<CBT01101DTO> JournalDetailGrid { get; set; } = new ObservableCollection<CBT01101DTO>();
        public ObservableCollection<CBT01101DTO> JournalDetailGridTemp { get; set; } = new ObservableCollection<CBT01101DTO>();
        #endregion

        public async Task GetAllUniversalData()
        {
            var loEx = new R_Exception();

            try
            {
                // Get Universal Data
                //var loResult = await _CBT01100InitModel.GetTabJournalEntryUniversalVarAsync();

                //Set Universal Data
                VAR_GSM_COMPANY = await _CBT01100InitModel.GetGSCompanyInfoAsync();
                VAR_CB_SYSTEM_PARAM = await _CBT01100InitModel.GetCBSystemParamAsync();
                VAR_GL_SYSTEM_PARAM = await _CBT01100InitModel.GetGLSystemParamAsync();
                VAR_GSM_TRANSACTION_CODE = await _CBT01100InitModel.GetGSTransCodeInfoAsync();
                VAR_TODAY = await _CBT01100InitModel.GetTodayDateAsync();
                VAR_CURRENCY_LIST = await _CBT01100InitModel.GetCurrencyListAsync();
                VAR_IUNDO_COMMIT_JRN = await _CBT01100InitModel.GetGSSystemEnableOptionInfoAsync();
                VAR_GSM_PERIOD = await _CBT01100InitModel.GetGSPeriodYearRangeAsync();
                VAR_GSB_CODE_LIST = await _CBT01100InitModel.GetGSBCodeListAsync();

                var loParam = new CBT01100ParamGSPeriodDTInfoDTO() { CCYEAR = VAR_CB_SYSTEM_PARAM.CCURRENT_PERIOD_YY, CPERIOD_NO = VAR_CB_SYSTEM_PARAM.CCURRENT_PERIOD_MM };
                VAR_CCURRENT_PERIOD_START_DATE = await _CBT01100InitModel.GetGSPeriodDTInfoAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Journal
        public async Task GetJournal(CBT01110DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01110Model.GetJournalRecordAsync(poEntity);

                RefDate = DateTime.ParseExact(loResult.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                DocDate = DateTime.ParseExact(loResult.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<CBT01110LastCurrencyRateDTO> GetLastCurrency(CBT01110LastCurrencyRateDTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01110LastCurrencyRateDTO loRtn = null;
            try
            {
                poEntity.CRATE_DATE = RefDate.ToString("yyyyMMdd");
                poEntity.CRATETYPE_CODE = VAR_GL_SYSTEM_PARAM.CRATETYPE_CODE;
                var loResult = await _CBT01110Model.GetLastCurrencyAsync(poEntity);

                loRtn = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public async Task UpdateJournalStatus(CBT01100UpdateStatusDTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await _CBT01100Model.UpdateJournalStatusAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task DeleteJournal(CBT01110DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = R_FrontUtility.ConvertObjectToObject<CBT01100UpdateStatusDTO>(poEntity);
                loData.LUNDO_COMMIT = false;
                loData.LAUTO_COMMIT = false;
                loData.CNEW_STATUS = "99";

                await UpdateJournalStatus(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveJournal(CBT01110DTO poEntity, eCRUDMode poCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    poEntity.CACTION = "NEW";
                    poEntity.CREC_ID = "";
                    poEntity.CREF_NO = VAR_GSM_TRANSACTION_CODE.LINCREMENT_FLAG ? "" : poEntity.CREF_NO;
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    poEntity.CACTION = "EDIT";
                }
                poEntity.CDOC_DATE = DocDate.Value.ToString("yyyyMMdd");
                poEntity.CREF_DATE = RefDate.ToString("yyyyMMdd");
                poEntity.CTRANS_CODE = ContextConstantCBT01100.VAR_TRANS_CODE;

                var loResult = await _CBT01110Model.SaveJournalAsync(poEntity);

                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Detail Journal
        public async Task GetJournalDetailList(CBT01101DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01100Model.GetJournalDetailListAsync(poEntity);
                loResult.ForEach(x => x.INO = loResult.Count + 1);

                JournalDetailGrid = new ObservableCollection<CBT01101DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveJournalDetail(CBT01111DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                _CBT01110Model.SaveJournalDetailAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                
            }loEx.ThrowExceptionIfErrors();
        }

        #endregion

    }
}
