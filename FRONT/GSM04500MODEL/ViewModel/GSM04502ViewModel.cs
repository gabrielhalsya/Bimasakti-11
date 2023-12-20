using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Enums;
using R_CommonFrontBackAPI;

namespace GSM04500Model
{
    public class GSM04502ViewModel : R_ViewModel<GOADeptDTO>
    {
        private GSM04502Model _modelGOADept = new GSM04502Model();
        public ObservableCollection<GOADeptDTO> GOADeptList = new ObservableCollection<GOADeptDTO>();
        public GOADeptDTO GOADept { get; set; } = new GOADeptDTO();
        public string GroupOfAccount { get; set; }

        public async Task GetGOAAllByDept(GOADTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _modelGOADept.GetAllGOADeptAsync(poEntity);
                GroupOfAccount = poEntity.GROUPOFACCOUNT;
                GOADeptList = new ObservableCollection<GOADeptDTO>(loResult.data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task <GOADeptDTO> GetGOADeptOneRecord (GOADeptDTO poEntity)
        {
            var loEx = new R_Exception();
            GOADeptDTO loResult = null;
            try
            {
                var loParam = new GOADeptDTO
                {
                    CCOMPANY_ID = poEntity.CCOMPANY_ID,
                    CUSER_ID = poEntity.CUSER_ID,
                    CPROPERTY_ID = poEntity.CPROPERTY_ID,
                    CJRNGRP_TYPE = poEntity.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poEntity.CJRNGRP_CODE,
                    CGOA_CODE = poEntity.CGOA_CODE,
                    CDEPT_CODE = poEntity.CDEPT_CODE
                };
                loResult = await _modelGOADept.R_ServiceGetRecordAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        
        public async Task<GOADeptDTO> SaveGOADept(GOADeptDTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GOADeptDTO loResult = null;

            try
            {
                loResult = await _modelGOADept.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                GOADept = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        public async Task DeleteOneRecordGOADept(GOADeptDTO poProperty)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new GOADeptDTO()
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE,
                    CGOA_CODE = poProperty.CGOA_CODE,
                    CDEPT_CODE = poProperty.CDEPT_CODE,
                    CGLACCOUNT_NO = poProperty.CGLACCOUNT_NO,
                    CUSER_ID = poProperty.CUSER_ID
                };
                await _modelGOADept.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

    }
}
