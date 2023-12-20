using GSM04500Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GSM04500Model
{
    public class GSM04502Model : R_BusinessObjectServiceClientBase<GOADeptDTO>, IGSM04510GOADept
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrl";
        private const string DEFAULT_ENDPOINT = "api/GSM04510GOADept";
        private const string DEFAULT_MODULE = "GS";

        public GSM04502Model(
            string pcHttpClientName = DEFAULT_HTTP,
            string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true)
            : base(pcHttpClientName, pcRequestServiceEndPoint, pcModule, plSendWithContext, plSendWithToken)
        {
        }


        public async Task<GOADeptResult> GetAllGOADeptAsync(GOADTO poEntity)
        {
            var loEx = new R_Exception();
            GOADeptResult loResult = new GOADeptResult();
            try
            {
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CJRNGRP_TYPE, poEntity.CJRNGRP_TYPE);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, poEntity.CPROPERTY_ID);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CJOURNAL_GRP_CODE, poEntity.CJRNGRP_CODE);
                R_BlazorFrontEnd.R_FrontContext.R_SetStreamingContext(ContextConstant.CGOA_CODE, poEntity.CGOA_CODE);
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTmp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GOADeptDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM04510GOADept.GetJournalGrpGOADeptList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.data = loTmp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public IAsyncEnumerable<GOADeptDTO> GetJournalGrpGOADeptList()
        {
            throw new NotImplementedException();
        }
    }
}
