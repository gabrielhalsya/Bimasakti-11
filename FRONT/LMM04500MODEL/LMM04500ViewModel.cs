﻿using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace LMM04500MODEL
{
    public class LMM04500ViewModel : R_ViewModel<UnitTypeCategoryDTO>
    {
        private LMM04500Model _modelPricing = new LMM04500Model();

        private const string PRICE_TYPE = "02"; //lease pricing code

        private const string CLASS_APPLICATION = "BIMASAKTI";

        public ObservableCollection<PropertyDTO> _propertyList { get; set; } = new ObservableCollection<PropertyDTO>();

        public ObservableCollection<UnitTypeCategoryDTO> _unitTypeCategoryList { get; set; } = new ObservableCollection<UnitTypeCategoryDTO>();

        public ObservableCollection<PricingDTO> _pricingList { get; set; } = new ObservableCollection<PricingDTO>();

        public ObservableCollection<PricingDTO> _pricingDateList { get; set; } = new ObservableCollection<PricingDTO>();

        public ObservableCollection<PricingBulkSaveDTO> _pricingSaveList { get; set; } = new ObservableCollection<PricingBulkSaveDTO>();

        public ObservableCollection<TypeDTO> _chargesTypeList { get; set; } = new ObservableCollection<TypeDTO>();
        public ObservableCollection<TypeDTO> _priceTypeList { get; set; } = new ObservableCollection<TypeDTO>();

        public enum eListPricingParamType { GetAll, GetNext, GetHistory }

        public string _currency { get; set; } = "";

        public string _propertyId { get; set; } = "";

        public string _unitTypeCategoryId { get; set; } = "";

        public string _unitTypeCategoryName { get; set; } = "";

        public string _validId { get; set; } = "";

        public DateTime _validDateForm { get; set; }= DateTime.Now;

        public string _validDate { get; set; } = "";

        public string _classId { get; set; } = "";

        public string _recIdCharList { get; set; } = "";
        
        public bool _tabNextPricingIsActive { get; set; } = false;

        public bool _tabHistoryPricingIsActive { get; set; } = false;

        public bool _tabPricingRateIsActive { get; set; } = false;

        public async Task GetPropertyList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                var loResult = await _modelPricing.GetPropertyListAsync();
                _propertyList = new ObservableCollection<PropertyDTO>(loResult);
                if (_propertyList.Count > 0)
                {
                    _propertyId = _propertyList.FirstOrDefault().CPROPERTY_ID;
                    _currency = $"{_propertyList.FirstOrDefault().CCURRENCY_NAME}({_propertyList.FirstOrDefault().CCURRENCY})";
                }
                else
                {
                    return;
                }
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
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                var loResult = await _modelPricing.GetUnitTypeCategoryListAsync();
                _unitTypeCategoryList = new ObservableCollection<UnitTypeCategoryDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetPricingList(eListPricingParamType poParam, bool llIsPricingDate)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                //lease pricing
                bool llActiveOnly = false;
                string lcType = "";
                switch (poParam)
                {
                    case eListPricingParamType.GetAll:
                        lcType = "01";
                        break;
                    case eListPricingParamType.GetNext:
                        lcType = "02";
                        break;
                    case eListPricingParamType.GetHistory:
                        lcType = "03";
                        break;
                }

                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID, _unitTypeCategoryId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, PRICE_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.LACTIVE, llActiveOnly);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CTYPE, lcType);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_ID, _validId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_DATE, _validDate);

                var loResult = llIsPricingDate ? await _modelPricing.GetPricingDateListAsync() : await _modelPricing.GetPricingListAsync();


                if (llIsPricingDate)
                {
                    _pricingDateList = new ObservableCollection<PricingDTO>(loResult);

                }
                else
                {
                    _pricingList = new ObservableCollection<PricingDTO>(loResult);
                }

                //if (_pricingList.Count<1)
                //{
                //    _validId = "";
                //    _validDate = "";
                //}
                //else
                //{
                //    _validId = _pricingList.FirstOrDefault().CVALID_INTERNAL_ID;
                //    _validDate = _pricingList.FirstOrDefault().CVALID_DATE;
                //}
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetPricingForSaveList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                bool llActiveOnly = false;
                string lcType = "01";
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPROPERTY_ID, _propertyId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CUNIT_TYPE_CATEGORY_ID, _unitTypeCategoryId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CPRICE_TYPE, PRICE_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.LACTIVE, llActiveOnly);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CTYPE, lcType);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_ID, _validId);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CVALID_DATE, _validDate);
                var loResultTemp = await _modelPricing.GetPricingListAsync();
                var loResult = R_FrontUtility.ConvertCollectionToCollection<PricingBulkSaveDTO>(loResultTemp);
                _pricingSaveList = new ObservableCollection<PricingBulkSaveDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetPriceType()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                //prepare param
                _classId = "_BS_PRICE_MODE";
                _recIdCharList = "";

                //send context
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CLASS_APPLICATION, CLASS_APPLICATION);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CLASS_ID, CLASS_APPLICATION);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.REC_ID_LIST, CLASS_APPLICATION);
                
                //get result
                var loResult = await _modelPricing.GetPriceChargesTypeAsync();
                
                //assign to list for grid
                _priceTypeList = new ObservableCollection<TypeDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetChargesType()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                //prepare param
                _classId = "_BS_UNIT_CHARGES_TYPE";
                _recIdCharList = "02, 05";

                //send context
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CLASS_APPLICATION, CLASS_APPLICATION);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.CLASS_ID, CLASS_APPLICATION);
                R_FrontContext.R_SetStreamingContext(ContextConstantLMM04500.REC_ID_LIST, CLASS_APPLICATION);
                
                //get result
                var loResult = await _modelPricing.GetPriceChargesTypeAsync();
                
                //assign to list for grid
                _chargesTypeList = new ObservableCollection<TypeDTO>(loResult);
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
                var loParam = new PricingSaveParamDTO()
                {

                    CPROPERTY_ID = _propertyId,
                    CPRICE_TYPE = PRICE_TYPE,
                    CUNIT_TYPE_CATEGORY_ID = _unitTypeCategoryId,
                    CVALID_FROM_DATE = _validDate,
                    CACTION = lcAction
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
