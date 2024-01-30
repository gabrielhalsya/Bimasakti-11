using LMM04500COMMON.DTO_s;
using LMM04500MODEL;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace LMM04500FRONT
{
    public partial class LMM04500 : R_Page
    {
        private LMM04500ViewModel _viewModelPricing = new();

        private R_Conductor _conUnitTypeCTG;
        private R_Grid<UnitTypeCategoryDTO> _gridUnitTypeCTG;

        private R_Conductor _conPricing;
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
                await (_viewModelPricing._propertyId != "" ? _gridUnitTypeCTG.R_RefreshGrid(null) : Task.CompletedTask);
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
                _viewModelPricing._propertyId = poParam;//re assign when property klicked on combobox

                if (_conUnitTypeCTG.R_ConductorMode == R_eConductorMode.Normal)
                {
                    await (_viewModelPricing._propertyId != "" ? _gridUnitTypeCTG.R_RefreshGrid(null) : Task.CompletedTask);

                    //sending property another tab (will be catch at init master)
                    switch (_tabStripPricing.ActiveTab.Id)
                    {
                        case "NP":
                            await _tabNextPricing.InvokeRefreshTabPageAsync(_viewModelPricing._propertyId);
                            break;
                        case "HP":
                            await _tabHistoryPricing.InvokeRefreshTabPageAsync(_viewModelPricing._propertyId);
                            break;
                        case "PR":
                            await _tabPricingRate.InvokeRefreshTabPageAsync(_viewModelPricing._propertyId);
                            break;
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
                eventArgs.Result = R_FrontUtility.ConvertObjectToObject<UnitTypeCategoryDTO>(eventArgs.Data);
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
                var loParam = R_FrontUtility.ConvertObjectToObject<UnitTypeCategoryDTO>(eventArgs.Data);
                _viewModelPricing._unitTypeCategoryId = loParam.CUNIT_TYPE_CATEGORY_ID;
                await _gridPricing.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Current Pricing

        private async Task CurrentPricing_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            { 
                await _viewModelPricing.GetPricingList(LMM04500ViewModel.ListPricingParamType.GetAll, false);
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
                eventArgs.Result = R_FrontUtility.ConvertObjectToObject<PricingDTO>(eventArgs.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private void CurrentPricing_Display(R_DisplayEventArgs eventArgs)
        {
            var loData = R_FrontUtility.ConvertObjectToObject<PricingDTO>(eventArgs.Data);
            _viewModelPricing._validDate = loData.CVALID_DATE;
            _viewModelPricing._validId = loData.CVALID_INTERNAL_ID;
        }

        #endregion

        #region Next pricing TabPage

        private void BeforeOpenTabPage_NextPricing(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(LMM04501);
            eventArgs.Parameter = _viewModelPricing._propertyId;
        }

        private void TabEventCallback_NextPricing(object poValue)
        {
            _comboboxPropertyEnabled = (bool)poValue;
            _pageNextPricingOnCRUDmode = !(bool)poValue;
        }

        #endregion

        #region History pricing TabPage

        private void BeforeOpenTabPage_HistoryPricing(R_BeforeOpenTabPageEventArgs eventArgs)
        {
            //eventArgs.TargetPageType = typeof(LMM04501);
            //eventArgs.Parameter = _viewModelPricing._propertyId;
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
