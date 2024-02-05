using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMM04500MODEL
{
    public class LMM04500Model : R_BusinessObjectServiceClientBase<PricingSaveParamDTO>, ILMM04500
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlLM";
        private const string DEFAULT_CHECKPOINT_NAME = "api/LMM04500";
        private const string DEFAULT_MODULE = "LM";
        public LMM04500Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_CHECKPOINT_NAME,
            bool plSendWithContext = true,
            bool plSendWithToken = true
            ) : base(
                pcHttpClientName,
                pcRequestServiceEndPoint,
                DEFAULT_MODULE,
                plSendWithContext,
                plSendWithToken)
        {
        }

        public async Task<List<PropertyDTO>> GetPropertyListAsync()
        {
            var loEx = new R_Exception();
            List<PropertyDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PropertyDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.GetPropertyList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;

        }

        public async Task<List<PricingDTO>> GetPricingDateListAsync()
        {
            var loEx = new R_Exception();
            List<PricingDTO> loResult = new List<PricingDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.GetPricingDateList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<UnitTypeCategoryDTO>> GetUnitTypeCategoryListAsync()
        {
            var loEx = new R_Exception();
            List<UnitTypeCategoryDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<UnitTypeCategoryDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.GetUnitTypeCategoryList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<PricingDTO>> GetPricingListAsync()
        {
            var loEx = new R_Exception();
            List<PricingDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.GetPricingList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<TypeDTO>> GetPriceChargesTypeAsync()
        {
            var loEx = new R_Exception();
            List<TypeDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<TypeDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.GetPriceChargesType),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task SavePricingAsync(PricingSaveParamDTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                await R_HTTPClientWrapper.R_APIRequestObject<PricingDumpResultDTO, PricingSaveParamDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04500.SavePricing),
                    poParam,
                    DEFAULT_MODULE
                    , _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        //NotImplementedException (Method)
        public IAsyncEnumerable<PropertyDTO> GetPropertyList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PricingDTO> GetPricingDateList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PricingDTO> GetPricingList()
        {
            throw new NotImplementedException();
        }

        public PricingDumpResultDTO SavePricing(PricingSaveParamDTO poParam)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TypeDTO> GetPriceChargesType()
        {
            throw new NotImplementedException();
        }
    }
}
