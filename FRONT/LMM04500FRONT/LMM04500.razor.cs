﻿using LMM04500COMMON.DTO_s;
using LMM04500MODEL;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500FRONT
{
    public partial class LMM04500:R_Page
    {
        private LMM04500ViewModel _viewModelPricing = new();

        private R_Conductor _conUnitTypeCTG; 
        private R_Grid<UnitTypeCategoryDTO> _gridUnitTypeCTG;

        private R_ConductorGrid _conPricing;
        private R_Grid<PricingDTO> _gridPricing;
        
        private R_TabStrip _tabStripPricing; //ref Tabstrip
        private R_TabPage _tabNextPricing; //tabpageNextPricing
        private R_TabPage _tabHistoryPricing; //tabpageNextPricing
        private R_TabPage _tabPricingRate; //tabpageNextPricing

        public bool _pageNextPricingOnCRUDmode = false; //to prevent moving tab while crudmode
        private bool _comboboxPropertyEnabled = true; //to prevent combobox while crudmode

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _viewModelPricing.GetPropertyList();
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        public async Task ComboboxPropertyValueChanged(string poParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                _viewModelPricing._pricingParam.CPROPERTY_ID= poParam;//re assign when property klicked on combobox

                if (_conPricing.R_ConductorMode == R_eConductorMode.Normal)
                {
                    await (_viewModelPricing._propertyList.Count >= 1 ? _gridUnitTypeCTG.R_RefreshGrid(null) : Task.CompletedTask);

                    if (_tabStripPricing.ActiveTab.Id == "TC")
                    {
                        //sending property ud to tab2 (will be catch at init master tab2)
                        await _tabNextPricing.InvokeRefreshTabPageAsync(_viewModelPricing._pricingParam.CPROPERTY_ID);
                        await _tabHistoryPricing.InvokeRefreshTabPageAsync(_viewModelPricing._pricingParam.CPROPERTY_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        
        private void OnActiveTabIndexChanging(R_TabStripActiveTabIndexChangingEventArgs eventArgs)
        {
            eventArgs.Cancel = _pageNextPricingOnCRUDmode;//prevent move to another tab
        }

        #region UnitTypeCategory

        private async Task UnitTypeCTG_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelPricing.GetUnitCategoryList();
                eventArgs.ListEntityResult = _viewModelPricing._unitTypeCategoryList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }

        private void UnitTypeCTG_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                eventArgs.Result = eventArgs.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task UnitTypeCTG_ServiceDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<PricingDTO>(eventArgs.Data);
                _viewModelPricing._pricingParam.CUNIT_TYPE_CATEGORY_ID = loParam.CUNIT_TYPE_CATEGORY_ID;
                await _gridPricing.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                throw;
            }
        }

        #endregion

        #region Current Pricing
        
        private async Task CurrentPricing_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelPricing.GetPricingList(LMM04500ViewModel.ListPricingParamType.GetAll,false);
                eventArgs.ListEntityResult = _viewModelPricing._pricingList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }

        private void CurrentPricing_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                eventArgs.Result = eventArgs.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Next pricing TabPage

        private void BeforeOpenTabPage_NextPricing(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(LMM03700Tab2);
            //eventArgs.Parameter = _viewTenantClassGrpModel._propertyId;
        }

        private void TabEventCallback_NextPricing(object poValue)
        {
            _comboboxPropertyEnabled = (bool)poValue;
            _pageNextPricingOnCRUDmode = !(bool)poValue;
        }
        
        #endregion

        #region Next pricing TabPage

        private void BeforeOpenTabPage_HistoryPricing(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(LMM03700Tab2);
            //eventArgs.Parameter = _viewTenantClassGrpModel._propertyId;
        }
        
        private void TabEventCallback_HistoryPricing(object poValue)
        {
            _comboboxPropertyEnabled = (bool)poValue;
            _pageNextPricingOnCRUDmode = !(bool)poValue;
        }

        #endregion

        #region pricing Rate TabPage

        private void BeforeOpenTabPage_PricingRate(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(LMM03700Tab2);
            //eventArgs.Parameter = _viewTenantClassGrpModel._propertyId;
        }

        private void TabEventCallback_PricingRate(object poValue)
        {
            _comboboxPropertyEnabled = (bool)poValue;
            _pageNextPricingOnCRUDmode = !(bool)poValue;
        }

        #endregion

    }
}