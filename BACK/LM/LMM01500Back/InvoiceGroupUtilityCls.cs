using LMM01500Common;
using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LMM01500Back
{
    public class InvoiceGroupUtilityCls
    {
        private LMM01500Logger _Logger;

        public InvoiceGroupUtilityCls()
        {
            _Logger = LMM01500Logger.R_GetInstanceLogger();
        }

        private void LogDebug(DbCommand pcCommand, [CallerMemberName] string callerMemberName = "")
        {
            _Logger.LogDebug($"Start method {callerMemberName} in {GetType().Name}. Command: {pcCommand.CommandText}. Parameters: {string.Join(", ", pcCommand.Parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName}={p.Value}"))}");
        }

        private void LogError(Exception ex, [CallerMemberName] string callerMemberName = "")
        {
            _Logger.LogError($"Error in {callerMemberName}: {ex.Message}", ex);
        }

        public List<PropertyDTO> GetAllPropertyList(InvoiceGroupParamDTO pcParam)
        {
            R_Exception loEx = new R_Exception();
            List<PropertyDTO> loResult = null;
            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection();
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_GS_GET_PROPERTY_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, pcParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, pcParam.CUSER_ID);
                //log
                LogDebug(loCmd);

                //exec rsp to database and get result
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<PropertyDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                LogError(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
    }
}
