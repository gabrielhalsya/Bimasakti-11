using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GSM04500Model
{
    public class GSM04501ViewModel : R_ViewModel<GOADTO>
    {
        private GSM04501Model _model = new GSM04501Model();
        public ObservableCollection<GOADTO> _GOAList = new ObservableCollection<GOADTO>();
        public GOADTO GOA { get; set; } = new GOADTO();

        public JournalDTO _currentJournalGroup = new JournalDTO();
        public GOADTO _currentGOA  = new GOADTO();
        public bool _enableGOADeptAddBtn=true;

        public bool _checkByDept = false;

        public async Task GetAllJournalGrupGOAAsync( string lcPropertyId, string lcJournalGRPType, string lcJournalGRPCode)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetAllGOAListAsync(lcJournalGRPType, lcPropertyId, lcJournalGRPCode);
                _GOAList = new ObservableCollection<GOADTO>(loResult.data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }

        public async Task<GOADTO> GetGOAOneRecord(GOADTO poEntity)
        {
            var loEx = new R_Exception();
            GOADTO loResult = null;
            try
            {
                var loParam = new GOADTO
                {
                    CCOMPANY_ID = poEntity.CCOMPANY_ID,
                    CUSER_ID = poEntity.CUSER_ID,
                    CPROPERTY_ID = poEntity.CPROPERTY_ID,
                    CJRNGRP_TYPE = poEntity.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poEntity.CJRNGRP_CODE,
                    CGOA_CODE = poEntity.CGOA_CODE
                };
                loResult = await _model.R_ServiceGetRecordAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<GOADTO> SaveGOA(GOADTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GOADTO loResult = null;

            try
            {
                loResult = await _model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                GOA = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}
