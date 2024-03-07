using LMT05000COMMON;
using R_BackEnd;
using R_Common;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;

namespace LMT05000BACK
{
    public class LMT05000Cls
    {
        private LogerLMT05000 _logger;

        private readonly ActivitySource _activitySource;

        public LMT05000Cls()
        {
            _logger = LogerLMT05000.R_GetInstanceLogger();
            _activitySource = LMT05000Activity.R_GetInstanceActivitySource();
        }

        #region log method helper

        private void ShowLogDebug(string query, DbParameterCollection parameters)
        {
            var paramValues = string.Join(", ", parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName} '{p.Value}'"));
            _logger.LogDebug($"EXEC {query} {paramValues}");
        }

        private void ShowLogError(Exception ex)
        {
            _logger.LogError(ex);
        }

        #endregion
    }
}
