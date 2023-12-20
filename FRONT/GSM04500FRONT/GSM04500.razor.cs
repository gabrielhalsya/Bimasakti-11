using BlazorClientHelper;
using GSM04500Common;
using GSM04500Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;

namespace GSM04500Front
{
    public partial class GSM04500
    {
        private GSM04500ViewModel _journalGroup_ViewModel = new();
        private R_ConductorGrid _jouralGroup_ConRef;
        private R_Grid<JournalDTO> _journalGroup_gridRef;
        private R_Conductor _journalGroup_PropertyConRef;
        private R_TabStrip _tabStrip;
        private R_TabPage _tabPageAccountSetting;
        [Inject] IJSRuntime _JSRuntime { get; set; }
        [Inject] IClientHelper _clientHelper { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalGroup_ViewModel.GetPropertyList();
                await _journalGroup_ViewModel.GetJournalGroupTypeList();
                await _journalGroup_gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task PropertyDropdown_OnChange(object poParam)
        {
            var loEx = new R_Exception();
            string lsProperty = (string)poParam;
            try
            {
                _journalGroup_ViewModel._PropertyValueContext = lsProperty;
                await _journalGroup_gridRef.R_RefreshGrid(poParam);
                _journalGroup_ViewModel._DropdownGroupType = true;

                if (_tabStrip.ActiveTab.Id == "Tab_AccountSetting")
                {
                    _journalGroup_ViewModel._DropdownGroupType = false;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task JournalGroupDropdown_OnChange(object poParam)
        {
            var loEx = new R_Exception();
            string lsJournalGrpType = (string)poParam;
            try
            {
                _journalGroup_ViewModel._JournalGroupTypeValue = lsJournalGrpType;
                _journalGroup_ViewModel._currentJournalGroup.CJRNGRP_TYPE = lsJournalGrpType;

                await _journalGroup_gridRef.R_RefreshGrid(poParam);
                _journalGroup_ViewModel._DropdownGroupType = true;

                if (_tabStrip.ActiveTab.Id == "Tab_AccountSetting")
                {
                    _journalGroup_ViewModel._DropdownGroupType = false;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        #region Journal Group
        private async Task JournalGroup_GetList(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _journalGroup_ViewModel.GetAllJournalAsync();
                eventArgs.ListEntityResult = _journalGroup_ViewModel._JournalGroupList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalGroup_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (JournalDTO)eventArgs.Data;
                eventArgs.Result = await _journalGroup_ViewModel.GetGroupJournaltOneRecord(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void JournalGroup_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (JournalDTO)eventArgs.Data;
                _journalGroup_ViewModel._currentJournalGroup = loParam;
            }
        }
        private void JournalGroup_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var loData = (JournalDTO)eventArgs.Data;
            loData.DCREATE_DATE = DateTime.Now;
            loData.DUPDATE_DATE = DateTime.Now;
        }
        private void JournalGroup_Saving(R_SavingEventArgs eventArgs)
        {
            var loData = (JournalDTO)eventArgs.Data;
            loData.CJRNGRP_CODE = string.IsNullOrWhiteSpace(loData.CJRNGRP_CODE) ? "" : loData.CJRNGRP_CODE;
            loData.CJRNGRP_NAME = string.IsNullOrWhiteSpace(loData.CJRNGRP_NAME) ? "" : loData.CJRNGRP_NAME;
        }
        private async Task JournalGroup_Delete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (JournalDTO)eventArgs.Data;
                await _journalGroup_ViewModel.DeleteOneRecordJournalGroup(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalGroup_Save(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (JournalDTO)eventArgs.Data;
                await _journalGroup_ViewModel.SaveJournalGroup(loParam, eventArgs.ConductorMode);

                eventArgs.Result = _journalGroup_ViewModel._JournalGroup;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region changetab
        private void AccSettingTab_BeforeOpenTabPage(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM04500AccountSetting); ;
            eventArgs.Parameter = _journalGroup_ViewModel._currentJournalGroup;
        }
        private void JournalGroup_OnTabChange(R_TabStripActiveTabIndexChangingEventArgs eventArgs)
        {
            _journalGroup_ViewModel._DropdownProperty = true;
            _journalGroup_ViewModel._DropdownGroupType = true;
            if (eventArgs.TabStripTab.Id == "Tab_AccountSetting")
            {
                _journalGroup_ViewModel._DropdownProperty = false;
                _journalGroup_ViewModel._DropdownGroupType = false;
            }
        }
        #endregion

        #region template
        private async Task BtnTemplate()
        {
            var loEx = new R_Exception();
            string loCompanyName = _clientHelper.CompanyId.ToUpper();

            try
            {
                var loValidate = await R_MessageBox.Show("", "Are you sure download this template?", R_eMessageBoxButtonType.YesNo);
                if (loValidate == R_eMessageBoxResult.Yes)
                {
                    var loByteFile = await _journalGroup_ViewModel.DownloadTemplate();
                    var saveFileName = $"Journal Group - {loCompanyName}.xlsx";
                    await _JSRuntime.downloadFileFromStreamHandler(saveFileName, loByteFile.FileBytes);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion

        #region upload
        private void JournalGrpUpload_BeforeOpenPopup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            string propertyId = _journalGroup_ViewModel._PropertyValueContext;
            PropertyDTO loparam = (_journalGroup_ViewModel._PropertyList).Find(p => p.CPROPERTY_ID == propertyId);

            var param = new JournalParamDTO()
            {
                CCOMPANY_ID = _journalGroup_ViewModel._currentJournalGroup.CCOMPANY_ID,
                CUSER_ID = _clientHelper.UserId,
                CJRNGRP_TYPE = _journalGroup_ViewModel._currentJournalGroup.CJRNGRP_TYPE,
                CPROPERTY_ID = loparam.CPROPERTY_ID,
                CPROPERTY_NAME = loparam.CPROPERTY_NAME
            };

            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSM04500Upload);
        }
        private async Task JournalGrpUpload_AfterOpenPopup(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                if (eventArgs.Success == false)
                {
                    return;
                }
                if ((bool)eventArgs.Result == true)
                {
                    await _journalGroup_gridRef.R_RefreshGrid(null);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion
    }
}
