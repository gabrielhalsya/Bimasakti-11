using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500MODEL
{
    public class LMM04501ViewModel : R_ViewModel<PricingRateSaveParamDTO>
    {
        private LMM04501Model _modelPricing = new LMM04501Model();
        private string _unitTypeCategoryId { get; set; } = "";
        private const string _priceType = "02"; //lease pricing code

        public ObservableCollection<PricingRateDTO> _pricingList { get; set; } = new ObservableCollection<PricingRateDTO>();

        public ObservableCollection<PricingRateDTO> _pricingDateList { get; set; } = new ObservableCollection<PricingRateDTO>();

        public ObservableCollection<PricingRateBulkSaveDTO> _pricingSaveList { get; set; } = new ObservableCollection<PricingRateBulkSaveDTO>();

        public string _propertyId { get; set; } = "";
        public string _pricingRateDate { get; set; } = "";


        public async Task GetPricingRateList(bool llIsPricingDate)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                //lease pricing
                string lcType = "";

                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, _priceType);                
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, _priceType);
                var loResult = await _modelPricing.GetPricingRateListAsync();

                _pricingList = new ObservableCollection<PricingRateDTO>(loResult);

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
                    CVALID_FROM_DATE = _validDate,
                    CVALID_INTERNAL_ID = _validId,
                    CACTION = lcAction,
                    CPRICE_TYPE = _priceType
                };

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
