using LMM01500Common;
using LMM01500Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_CommonFrontBackAPI;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LMM01500Services
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LMM01500Controller : ControllerBase, ILMM01500
    {
        private LMM01500Logger _loggerLMM01500;
        public LMM01500Controller(ILogger<LMM01500Controller> logger)
        {
            LMM01500Logger.R_InitializeLogger(logger);
            _loggerLMM01500 = LMM01500Logger.R_GetInstanceLogger();
        }

        private void ShowLogInfo([CallerMemberName] string pcMethodName = "")
        {
            _loggerLMM01500.LogInfo($"Start method {pcMethodName} in {GetType().Name}");
        }

        public IAsyncEnumerable<InvoiceGroupDTO> GetInvoiceGroupList()
        {
            ShowLogInfo();
            throw new NotImplementedException();
        }

        private static async IAsyncEnumerable<T> StreamHelper<T>(List<T> dtoList) where T : class
        {
            foreach (T dtoEntity in dtoList)
            {
                yield return dtoEntity;
            }
        }

        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<InvoiceGroupDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceGetRecordResultDTO<InvoiceGroupDTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<InvoiceGroupDTO> poParameter)
        {
            throw new NotImplementedException();
        }

        public R_ServiceSaveResultDTO<InvoiceGroupDTO> R_ServiceSave(R_ServiceSaveParameterDTO<InvoiceGroupDTO> poParameter)
        {
            throw new NotImplementedException();
        }
    }
}
