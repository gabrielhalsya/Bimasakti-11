using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Runtime.CompilerServices;
using R_OpenTelemetry;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using LMT05000COMMON;
using LMT05000COMMON.DTO_s;
using LMT05000BACK;
namespace LMT05000SERVICE
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LMT050000Controller : ControllerBase, ILMM05000
    {
        private LogerLMT05000 _logger;
        
        private readonly ActivitySource _activitySource;

        public LMT050000Controller(ILogger<LMT050000Controller> logger)
        {
            //initiate
            LogerLMT05000.R_InitializeLogger(logger);
            _logger = LogerLMT05000.R_GetInstanceLogger();
            _activitySource = LMT05000Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        [HttpPost]
        public IAsyncEnumerable<AgreementChrgDiscDetailDTO> GetAgreementChargesDiscountList(AgreementChrgDiscParamDTO poParam)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public AgreementChrgDiscResultDTO ProcessAgreementChargeDiscount(AgreementChrgDiscParamDTO popaParam)
        {
            throw new NotImplementedException();
        }

        #region logger
        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);
        #endregion
    }
}
