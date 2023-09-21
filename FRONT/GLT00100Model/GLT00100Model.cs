using GLT00100Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace GLT00100Model
{
    public class GLT00100Model : R_BusinessObjectServiceClientBase<GLT00100DTO>, IGLT00100
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlGL";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GLT00100";
        private const string DEFAULT_MODULE = "GL";
        public GLT00100Model() :
            base(DEFAULT_HTTP_NAME, DEFAULT_SERVICEPOINT_NAME, DEFAULT_MODULE, true, true)
        {
        }

        public IAsyncEnumerable<GLT00100DetailDTO> GetAllJournalDetailList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GLT00100GridDTO> GetJournalList()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GLT00100DetailDTO>> GetJournalDetailListAsync()
        {
            R_Exception loEx = new R_Exception();
            List<GLT00100DetailDTO> loResult = new List<GLT00100DetailDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                var loResTemp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GLT00100DetailDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00100.GetAllJournalDetailList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult= loResTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task<List<GLT00100GridDTO>> GetJournalListAsync()
        {
            R_Exception loEx = new R_Exception();
            List<GLT00100GridDTO> loResult = new List<GLT00100GridDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GLT00100GridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00100.GetJournalList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}
