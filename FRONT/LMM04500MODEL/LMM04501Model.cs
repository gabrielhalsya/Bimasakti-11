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
    public class LMM04501Model : R_BusinessObjectServiceClientBase<PricingRateSaveParamDTO>, ILMM04501
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlLM";
        private const string DEFAULT_CHECKPOINT_NAME = "api/LMM04501";
        private const string DEFAULT_MODULE = "LM";
        public LMM04501Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
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

        public async Task<List<PricingRateDTO>> GetPricingRateDateListAsync()
        {
            var loEx = new R_Exception();
            List<PricingRateDTO> loResult = new List<PricingRateDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingRateDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04501.GetPricingRateDateList),
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

        public async Task<List<PricingRateDTO>> GetPricingRateListAsync()
        {
            var loEx = new R_Exception();
            List<PricingRateDTO> loResult = new List<PricingRateDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingRateDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04501.GetPricingRateList),
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

        public async Task SavePricingRateAsync(PricingRateSaveParamDTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                await R_HTTPClientWrapper.R_APIRequestObject<PricingDumpResultDTO, PricingRateSaveParamDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM04501.SavePricingRate),
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

        public IAsyncEnumerable<PricingRateDTO> GetPricingRateDateList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PricingRateDTO> GetPricingRateList()
        {
            throw new NotImplementedException();
        }

        public PricingDumpResultDTO SavePricingRate(PricingRateSaveParamDTO poParam)
        {
            throw new NotImplementedException();
        }


    }
}
