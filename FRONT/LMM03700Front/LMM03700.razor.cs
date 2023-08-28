using LMM03700Common;
using LMM03700Common.DTO_s;
using LMM03700Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace LMM03700Front
{
    public partial class LMM03700 : R_Page
    {
        private LMM03700ViewModel _vmTenantClassGrp = new();
        private LMM03710ViewModel _vmTenantClass = new();
        private R_ConductorGrid _conTenantClassGrp; //conductor grid tenantclassgrp tab 1
        private R_Grid<TenantClassificationGroupDTO> _gridTenantClassGrp; //gridref  tenantclassgrp tab 1 

        private R_TabPage _tabPageTenantClass; //refTabPageTab2
        private R_TabStrip _tabStrip; //refTabstrip

        public bool _pageTenantClassOnCRUDmode = false; //to disable moving tab while crudmode
        private bool _comboboxPropertyEnabled = true; //to disable combobox while crudmode

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await Property_ServiceGetListRecord(null);
                await _gridTenantClassGrp.R_RefreshGrid(null); //refresh grid tab 1
                //await _gridT2_TCGRef.R_RefreshGrid(null); //refresh grid tab 2
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }

        #region PropertyDropdown
        private async Task Property_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _vmTenantClassGrp.GetPropertyList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
        private async void ComboboxPropertyOnChanged()
        {
            if (_conTenantClassGrp.R_ConductorMode == R_eConductorMode.Normal)
            {
                _vmTenantClass._propertyId = _vmTenantClassGrp._propertyId; //assign property_id as param grid
                await _gridTenantClassGrp.R_RefreshGrid(null); //refresh grid tab 1

                if (_tabStrip.ActiveTab.Id == "TC")
                {
                    await _tabPageTenantClass.InvokeRefreshTabPageAsync(_vmTenantClass._propertyId);
                }
            }
        }
        #endregion

        #region TabPage
        private void R_Before_Open_TabPage(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(LMM03700Tab2);
            eventArgs.Parameter = _vmTenantClassGrp._propertyId;
        }
        private void R_After_Open_TabPage(R_AfterOpenTabPageEventArgs eventArgs)
        {

        }
        private void R_TabEventCallback(object poValue)
        {
            _comboboxPropertyEnabled = (bool)poValue;
            _pageTenantClassOnCRUDmode = !(bool)poValue;
        }
        #endregion

        #region TabSet
        private void OnActiveTabIndexChanging(R_TabStripActiveTabIndexChangingEventArgs eventArgs)
        {
            eventArgs.Cancel = _pageTenantClassOnCRUDmode;
        }
        #endregion

        #region Tab1-TenantClassificationGroup
        private async Task TenantClassGrp_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _vmTenantClassGrp.GetTenantClassGroupList();
                eventArgs.ListEntityResult = _vmTenantClassGrp.TenantClassificationGroupList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
        private async Task TenantClassGrp_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationGroupDTO>(eventArgs.Data);
                await _vmTenantClassGrp.GetTenantClassGroupRecord(loParam);
                eventArgs.Result = _vmTenantClassGrp.TenantClassificationGroup;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task TenantClassGrp_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationGroupDTO>(eventArgs.Data);
                await _vmTenantClassGrp.DeleteTenantClassGroup(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task TenantClassGrp_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationGroupDTO>(eventArgs.Data);
                await _vmTenantClassGrp.SaveTenantClassGroup(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _vmTenantClassGrp.TenantClassificationGroup;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private void TenantClassGrp_Validation(R_ValidationEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (TenantClassificationGroupDTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.CTENANT_CLASSIFICATION_GROUP_ID))
                    loEx.Add("", "Please fill Tenant Classification Group Id ");

                if (string.IsNullOrWhiteSpace(loData.CTENANT_CLASSIFICATION_GROUP_NAME))
                    loEx.Add("", "Please fill Tenant Classification Group Name ");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

    }

}
