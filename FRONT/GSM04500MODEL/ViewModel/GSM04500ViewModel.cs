using GSM04500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GSM04500FrontResources;
using R_BlazorFrontEnd.Helpers;

namespace GSM04500Model
{
    public class GSM04500ViewModel : R_ViewModel<JournalDTO>
    {
        private GSM04500Model _model = new GSM04500Model();
        private GSM04500ModelTemplate _modelUploadTemplate = new GSM04500ModelTemplate();
        public List<PropertyDTO> _PropertyList { get; set; } = new List<PropertyDTO>();
        public List<JournalGrpTypeDTO> _JournalGroupTypeList { get; set; } = new List<JournalGrpTypeDTO>();
        
        public ObservableCollection<JournalDTO> _JournalGroupList = new ObservableCollection<JournalDTO>();
        public JournalDTO _JournalGroup { get; set; } = new JournalDTO();
        public PropertyDTO _currentProperty = new PropertyDTO();
        public JournalDTO _currentJournalGroup  = new JournalDTO();

        public string _PropertyValueContext = "";
        public string _JournalGroupTypeValue = "";
        public string _JournalGroupCodeValue = "";
        public bool _DropdownGroupType = true;
        public bool _DropdownProperty = true;
        public bool VisibleColumn_LACCRUAL = false;

        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                _PropertyList = await _model.GetPropertyListAsync();
                _currentProperty = _PropertyList.FirstOrDefault();
                _PropertyValueContext = _currentProperty.CPROPERTY_ID;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetJournalGroupTypeList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GetJournalGroupTypeListAsyncModel();
                _JournalGroupTypeList = loResult.Data;
                _JournalGroupTypeValue = _JournalGroupTypeList.FirstOrDefault().CCODE;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetAllJournalAsync()
        {
            R_Exception loException = new R_Exception();
            try
            {
                switch (_JournalGroupTypeValue)
                {
                    case "10":
                    case "11":
                    case "40":
                        VisibleColumn_LACCRUAL = true;
                        break;
                    default:
                        VisibleColumn_LACCRUAL = false;
                        break;
                }

                var x = VisibleColumn_LACCRUAL;
                var loResult = await _model.GetAllJournalGroupListAsync(_JournalGroupTypeValue, _PropertyValueContext);
                _JournalGroupList = new ObservableCollection<JournalDTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();
        }
        public async Task<JournalDTO> GetGroupJournaltOneRecord(JournalDTO poProperty)
        {
            var loEx = new R_Exception();
            JournalDTO loResult = null;

            try
            {
                var loParam = new JournalDTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CUSER_ID = poProperty.CUSER_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE
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
        public async Task DeleteOneRecordJournalGroup(JournalDTO poProperty)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new JournalDTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CJRNGRP_TYPE = poProperty.CJRNGRP_TYPE,
                    CJRNGRP_CODE = poProperty.CJRNGRP_CODE,
                    CJRNGRP_NAME = poProperty.CJRNGRP_NAME,
                    LACCRUAL = poProperty.LACCRUAL
                };
                await _model.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task<JournalDTO> SaveJournalGroup(JournalDTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            JournalDTO loReturn = null;
            try
            {
                poNewEntity.CPROPERTY_ID = _PropertyValueContext;
                poNewEntity.CJRNGRP_TYPE = _JournalGroupTypeValue;
                loReturn = await _model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
                _JournalGroup = loReturn;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loReturn;
        }

        #region Template
        public async Task<UploadFileDTO> DownloadTemplate()
        {
            var loEx = new R_Exception();
            UploadFileDTO loResult = null;
            try
            {
                loResult = await _modelUploadTemplate.DownloadTemplateFileAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        #endregion
    }
}
