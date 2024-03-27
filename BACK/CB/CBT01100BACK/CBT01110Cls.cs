using CBT01100COMMON;
using R_BackEnd;
using R_Common;
using System.Data.Common;
using System.Data;
using R_CommonFrontBackAPI;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;
using CBT01100COMMON.Loggers;
using CBT01100COMMON.DTO_s;

namespace CBT01100BACK
{
    public class CBT01110Cls
    {
        private RSP_CB_DELETE_TRANS_JRNResources.Resources_Dummy_Class loDeleteCBTransJRNRes = new();
        private RSP_CB_SAVE_TRANS_JRNResources.Resources_Dummy_Class loSaveCBTransJRNRes = new();
        private RSP_CB_SAVE_CA_WT_JOURNALResources.Resources_Dummy_Class loSaveCAWTJRNRes = new();
        private RSP_CB_SAVE_SYSTEM_PARAMResources.Resources_Dummy_Class loSaveSystemCBTransJRNRes = new();
        private RSP_CB_UPDATE_TRANS_HD_STATUSResources.Resources_Dummy_Class loUpdateCBTransHDStatusRes
            = new();
        private LoggerCBT01100 _logger;

        private readonly ActivitySource _activitySource;
        public CBT01110Cls()
        {
            _logger = LoggerCBT01100.R_GetInstanceLogger();
            _activitySource = CBT01100Activity.R_GetInstanceActivitySource();
        }

        public CBT01110LastCurrencyRateDTO GetLastCurrency(CBT01110LastCurrencyRateDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            CBT01110LastCurrencyRateDTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_GS_GET_LAST_CURRENCY_RATE";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, string.IsNullOrWhiteSpace(poEntity.CCURRENCY_CODE) ? "" : poEntity.CCURRENCY_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CRATETYPE_CODE", DbType.String, 50, poEntity.CRATETYPE_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CRATE_DATE", DbType.String, 50, string.IsNullOrWhiteSpace(poEntity.CRATE_DATE) ? "" : poEntity.CRATE_DATE);

                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01110LastCurrencyRateDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public CBT01110DTO GetJournalDisplay(CBT01110DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            CBT01110DTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_GL_GET_JOURNAL";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 100, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, R_BackGlobalVar.CULTURE);

                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01110DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public CBT01110DTO SaveJournal(CBT01110DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd = null;
            CBT01110DTO loRtn = null;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loConn = loDb.GetConnection("R_DefaultConnectionString");
                    loCmd = loDb.GetCommand();
                    lcQuery = "RSP_CB_SAVE_CA_WT_JOURNAL";
                    loCmd.CommandText = lcQuery;
                    loCmd.CommandType = CommandType.StoredProcedure;

                    loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, R_BackGlobalVar.USER_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CJRN_ID", DbType.String, 100, poEntity.CREC_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, poEntity.CACTION);
                    loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, R_BackGlobalVar.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 20, poEntity.CDEPT_CODE);
                    loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, 20, ContextConstantCBT01100.VAR_TRANS_CODE);
                    loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, 50, poEntity.CREF_NO);
                    loDb.R_AddCommandParameter(loCmd, "@CDOC_NO", DbType.String, 50, poEntity.CDOC_NO);
                    loDb.R_AddCommandParameter(loCmd, "@CDOC_DATE", DbType.String, 10, poEntity.CDOC_DATE);
                    loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String, 10, poEntity.CREF_DATE);
                    loDb.R_AddCommandParameter(loCmd, "@CREVERSE_DATE", DbType.String, 10, "");
                    loDb.R_AddCommandParameter(loCmd, "@LREVERSE", DbType.Boolean, 10, poEntity.LREVERSE);
                    loDb.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, int.MaxValue, poEntity.CTRANS_DESC);
                    loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, poEntity.CCURRENCY_CODE);
                    loDb.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, 100, poEntity.NLBASE_RATE);
                    loDb.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, 100, poEntity.NLCURRENCY_RATE);
                    loDb.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, 100, poEntity.NBBASE_RATE);
                    loDb.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, 100, poEntity.NBCURRENCY_RATE);
                    loDb.R_AddCommandParameter(loCmd, "@NPRELIST_AMOUNT", DbType.Decimal, 100, poEntity.NPRELIST_AMOUNT);


                    R_ExternalException.R_SP_Init_Exception(loConn);

                    try
                    {
                        ShowLogDebug(lcQuery, loCmd.Parameters);
                        var loDataTable = loDb.SqlExecQuery(loConn, loCmd, false);

                        var loTempResult = R_Utility.R_ConvertTo<ConvertRecID>(loDataTable).FirstOrDefault();

                        loRtn.CREC_ID = loTempResult.CJRN_ID;
                    }
                    catch (Exception ex)
                    {
                        loEx.Add(ex);
                    }

                    loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
                if (loCmd != null)
                {
                    loCmd.Dispose();
                    loCmd = null;
                }
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public CBT01110DTO SaveJournalDetail(CBT01111DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            CBT01110DTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_CB_SAVE_TRANS_JRN";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String,int.MaxValue, poEntity.CACTION);
                loDb.R_AddCommandParameter(loCmd, "@CPARENT_ID", DbType.String,int.MaxValue, poEntity.CPARENT_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String,int.MaxValue, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String,int.MaxValue, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String,int.MaxValue, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String,int.MaxValue, ContextConstantCBT01100.VAR_TRANS_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String,int.MaxValue, poEntity.CREF_NO);
                loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String,int.MaxValue, poEntity.CREF_DATE);
                loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String,int.MaxValue, poEntity.CREF_DATE);
                loDb.R_AddCommandParameter(loCmd, "@CINPUT_TYPE", DbType.String,int.MaxValue, poEntity.CINPUT_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String,int.MaxValue, poEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CCENTER_CODE", DbType.String,int.MaxValue, poEntity.CCENTER_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCASH_FLOW_GROUP_CODE", DbType.String,int.MaxValue, poEntity.CCASH_FLOW_GROUP_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCASH_FLOW_CODE", DbType.String,int.MaxValue, poEntity.CCASH_FLOW_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDBCR", DbType.String,int.MaxValue, poEntity.CDBCR);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String,int.MaxValue, poEntity.CCURRENCY_CODE);
                loDb.R_AddCommandParameter(loCmd, "@NLTRANS_AMOUNT", DbType.Decimal,int.MaxValue, poEntity.NLTRANS_AMOUNT);
                loDb.R_AddCommandParameter(loCmd, "@CDETAIL_DESC", DbType.String,int.MaxValue, poEntity.CDETAIL_DESC);
                loDb.R_AddCommandParameter(loCmd, "@CDOCUMENT_NO", DbType.String,int.MaxValue, poEntity.CDOCUMENT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CDOCUMENT_DATE", DbType.String,int.MaxValue, poEntity.CDOCUMENT_DATE);
                loDb.R_AddCommandParameter(loCmd, "@LSUSPENSE_ACCOUNT", DbType.Boolean,int.MaxValue, poEntity.LSUSPENSE_ACCOUNT);

                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                var loTempResult = R_Utility.R_ConvertTo<ConvertRecID>(loDataTable).FirstOrDefault();
                loResult.CREC_ID = loTempResult.CPARENT_ID;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        #region log activity
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
    internal class ConvertRecID
    {
        public string CJRN_ID { get; set; }
        public string CPARENT_ID { get; set; }
    }





}