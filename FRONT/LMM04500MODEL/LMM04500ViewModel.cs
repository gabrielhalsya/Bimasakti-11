using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500MODEL
{
    public class LMM04500ViewModel : R_ViewModel<PricingSaveParamDTO>
    {
        private LMM04500Model _modelPricing = new LMM04500Model();

        public ObservableCollection<PropertyDTO> _propertyList { get; set; } = new ObservableCollection<PropertyDTO>();

        public ObservableCollection<UnitTypeCategoryDTO> _unitTypeCategoryList { get; set; } = new ObservableCollection<UnitTypeCategoryDTO>();

        public ObservableCollection<PricingDTO> _pricingList { get; set; } = new ObservableCollection<PricingDTO>();

        public ObservableCollection<PricingDTO> _pricingDateList { get; set; } = new ObservableCollection<PricingDTO>();

        public ObservableCollection<PricingBulkSaveDTO> _pricingSaveList { get; set; } = new ObservableCollection<PricingBulkSaveDTO>();

        public enum ListPricingParamType { GetAll = 1, GetNext = 2, GetHistory = 3 }

        public PricingDTO _pricing { get; set; } = new PricingDTO();

        public PricingParamDTO _pricingParam { get; set; } = new PricingParamDTO()
        {
            CPROPERTY_ID = "",
            CUNIT_TYPE_CATEGORY_ID = "",
            CVALID_INTERNAL_ID = "",
            CVALID_DATE = "",
            CVALID_FROM_DATE = ""
        };

        public bool _tabNextPricingIsActive { get; set; } = false;
        
        public bool _tabHistoryPricingIsActive { get; set; } = false;
        
        public bool _tabPricingRateIsActive { get; set; } = false;

        public async Task GetPropertyList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _pricingParam.CPROPERTY_ID);
                var loResult = await _modelPricing.GetPropertyListAsync();
                _propertyList = new ObservableCollection<PropertyDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetUnitCategoryList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _pricingParam.CPROPERTY_ID);
                var loResult = await _modelPricing.GetUnitTypeCategoryListAsync();
                _unitTypeCategoryList = new ObservableCollection<UnitTypeCategoryDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetPricingList(ListPricingParamType poParam, bool llIsPricingDate)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                string lcType = poParam.ToString("D2"); //CTYPE ()
                string lcPriceType = "02"; //lease pricing
                bool llActiveOnly = false;

                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _pricingParam.CPROPERTY_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID, _pricingParam.CUNIT_TYPE_CATEGORY_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, lcPriceType);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.LACTIVE, llActiveOnly);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CTYPE, lcType);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_ID, _pricingParam.CVALID_INTERNAL_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_DATE, _pricingParam.CVALID_DATE);

                var loResult = llIsPricingDate ? await _modelPricing.GetPricingDateListAsync() : await _modelPricing.GetPricingListAsync();
                _pricingList = new ObservableCollection<PricingDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task SavePricing()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loParam = new PricingSaveParamDTO();
                loParam = R_FrontUtility.ConvertObjectToObject<PricingSaveParamDTO>(_pricingParam);
                loParam.PRICING_LIST = new List<PricingBulkSaveDTO>(_pricingSaveList);
                await _modelPricing.SavePricingAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }
}
