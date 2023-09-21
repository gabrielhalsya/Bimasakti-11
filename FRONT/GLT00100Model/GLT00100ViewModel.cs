using GLT00100Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GLT00100Model
{
    public class GLT00100ViewModel : R_ViewModel<GLT00100DTO>
    {
        public const string CTRANS_CODE = "000000";
        public GLT00100Model _modelGLT00100 { get; set; }
        public ObservableCollection<GLT00100GridDTO> _journalList { get; set; } = new ObservableCollection<GLT00100GridDTO>();
        public ObservableCollection<GLT00100DetailDTO> _journalDetailList { get; set; } = new ObservableCollection<GLT00100DetailDTO>();
        public ObservableCollection<GLT00100DetailDTO> _journaDetailListTemp { get; set; } = new ObservableCollection<GLT00100DetailDTO>();
        public GLT00100DTO _journal { get; set; } = new GLT00100DTO();
        public GLT00100ParamDTO _searchParam { get; set; }
        public string CREC_ID { get; set; } = "";
        public List<CenterDTO> _CenterListData { get; set; } = new List<CenterDTO>();

        public async Task GetJournal(GLT00100DTO loParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _modelGLT00100.R_ServiceGetRecordAsync(loParam);
                _journal = loResult;

                _journal.DREF_DATE = DateTime.ParseExact(_journal.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                _journal.DDOC_DATE = DateTime.ParseExact(_journal.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                //_journal.CUPDATE_DATE = _journal.DUPDATE_DATE.ToLongDateString();
                //_journal.CCREATE_DATE = _journal.DCREATE_DATE.ToLongDateString();
                //LcCrecID = _journal.CREC_ID;
                //Data.CSTATUS = _journal.CSTATUS;
                await GetJournalDetailList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveJournal(GLT00100DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
            try
            {
                poNewEntity.CTRANS_CODE = CTRANS_CODE;
                var loResult = await _modelGLT00100.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                _journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task ShowAllJournals()
        {
            
            R_Exception loEx = new R_Exception();
            try
            {
                //lcPeriod = Data.ISOFT_PERIOD_YY + Data.CSOFT_PERIOD_MM;
                //lcSearch = Data.CSEARCH_TEXT ?? "";
                //lcDeptCode = Data.CDEPT_CODE;
                //lcStatus = Data.CSTATUS ?? "";
                R_FrontContext.R_SetContext(GLT00100ContextConstant.CPERIOD, _searchParam.CPERIOD);
                R_FrontContext.R_SetContext(GLT00100ContextConstant.CSEARCH_TEXT, _searchParam.CSEARCH_TEXT);
                R_FrontContext.R_SetContext(GLT00100ContextConstant.CDEPT_CODE, _searchParam.CDEPT_CODE);
                R_FrontContext.R_SetContext(GLT00100ContextConstant.CSTATUS, _searchParam.CSTATUS);
                var loResult = await _modelGLT00100.GetJournalListAsync();
                _journalList = new ObservableCollection<GLT00100GridDTO>(loResult);
                foreach (var item in _journalList)
                {
                    DateTime parsedCrefDate;
                    if (DateTime.TryParseExact(item.CREF_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedCrefDate))
                    {
                        item.DREF_DATE = parsedCrefDate;
                    }
                }

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
                R_FrontContext.R_SetContext(GLT00100ContextConstant.CREC_ID, CREC_ID);

                var loResult = await _modelGLT00100.GetJournalDetailListAsync();

                if (loResult != null)
                {
                    var modifiedList = loResult.Select((m, Index) =>
                    {
                        m.INO = Index + 1;
                        m.NAMOUNT = m.NDEBIT + m.NCREDIT;
                        if (m.CCENTER_CODE == null)
                        {
                            foreach (var item in _CenterListData)
                            {
                                if (m.CCENTER_NAME == item.CCENTER_NAME)
                                {
                                    m.CCENTER_CODE = item.CCENTER_CODE;
                                }
                            }
                        }
                        return m;
                    }).ToList();

                    _journalDetailList = new ObservableCollection<GLT00100DetailDTO>(modifiedList);
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }
}
