using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LMM04500MODEL
{
    public class LMM04501ViewModel : R_ViewModel<PricingRateDTO>
    {
        private LMM04501Model _modelPricing = new LMM04501Model();
        private string _unitTypeCategoryId { get; set; } = "";

        private const string PRICE_TYPE = "02"; //lease pricing code

        private const string UNITCTGID = "02"; //unit category id 

        public ObservableCollection<PricingRateDTO> _pricingRateList { get; set; } = new ObservableCollection<PricingRateDTO>();

        public ObservableCollection<PricingRateDTO> _pricingRateDateList { get; set; } = new ObservableCollection<PricingRateDTO>();

        public ObservableCollection<PricingRateBulkSaveDTO> _pricingSaveList { get; set; } = new ObservableCollection<PricingRateBulkSaveDTO>();

        public string _propertyId { get; set; } = "";
        
        public string _pricingRateDate { get; set; } = "";

        public async Task GetPricingRateDateList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, PRICE_TYPE);
                var loResult = await _modelPricing.GetPricingRateDateListAsync();
                _pricingRateDateList = new ObservableCollection<PricingRateDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetPricingRateList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID, UNITCTGID);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, PRICE_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CRATE_DATE, _pricingRateDate);
                var loResult = await _modelPricing.GetPricingRateListAsync();
                _pricingRateList = new ObservableCollection<PricingRateDTO>(loResult);
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
                string lcAction = "ADD";
                var loParam = new PricingRateSaveParamDTO()
                {

                    CPROPERTY_ID = _propertyId,
                    CUNIT_TYPE_CATEGORY_ID = _unitTypeCategoryId,
                    CPRICE_TYPE = PRICE_TYPE,
                    CACTION = lcAction,
                    CRATE_DATE = _pricingRateDate,
                };

                loParam.PRICING_RATE_LIST = new List<PricingRateBulkSaveDTO>(_pricingSaveList);
                await _modelPricing.SavePricingRateAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

    }
}
