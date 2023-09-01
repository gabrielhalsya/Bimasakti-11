using GLM00200Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;
using GLM00200Common.DTO_s;
using Microsoft.Extensions.Logging.EventSource;
using Castle.Core.Resource;
using System.Transactions;
using R_APICommonDTO;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Collections.Generic;
using System.Globalization;
using System.Data.SqlClient;

namespace GLM00200Back
{
    public class GLM00200Cls : R_BusinessObject<JournalParamDTO>
    {
        protected override void R_Deleting(JournalParamDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            string lcQuery = "";
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);

                lcQuery = "RSP_GL_UPDATE_JOURNAL_STATUS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CAPPROVE_BY", DbType.String, 20, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJRN_ID_LIST", DbType.String, 2047483647, poEntity.CJRN_ID);
                loDb.R_AddCommandParameter(loCmd, "@CNEW_STATUS", DbType.String, 2, "99");
                loDb.R_AddCommandParameter(loCmd, "@LAUTO_COMMIT", DbType.String, 2, "0");
                loDb.R_AddCommandParameter(loCmd, "@LUNDO_COMMIT", DbType.String, 2, "0");
                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }

                    loConn.Dispose();
                }
            }

        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        protected override JournalParamDTO R_Display(JournalParamDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            JournalParamDTO loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GL_GET_RECURRING_JRN";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                loDB.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 50, poEntity.CREC_ID);
                loDB.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, poEntity.CLANGUAGE_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<JournalParamDTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        protected override void R_Saving(JournalParamDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB = null;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loDB = new R_Db();
                    loCmd = loDB.GetCommand();
                    R_ExternalException.R_SP_Init_Exception(loConn);
                    loConn = loDB.GetConnection();

                    lcQuery = "CREATE TABLE #GLM00200_JOURNAL_DETAIL " +
                            "( " +
                            ", CGLACCOUNT_NO VARCHAR(20) " +
                            ", CCENTER_CODE NVARCHAR(10) " +
                            ", CDBCR CHAR(1) " +
                            ", NAMOUNT NUMERIC(19,2) " +
                            ", CDETAIL_DESC NVARCHAR(200) " +
                            ", CDOCUMENT_NO VARCHAR(20) " +
                            ", CDOCUMENT_DATE VARCHAR(8)" +
                            " ) ";
                    loDB.SqlExecNonQuery(lcQuery, loConn, false);

                    loDB.R_BulkInsert<JournalDetailGridDTO>((SqlConnection)loConn, "#GLM00200_JOURNAL_DETAIL", poNewEntity.ListJournalDetail);

                    lcQuery = "EXEC RSP_GL_SAVE_RECURRING_JRN " +
                        "@CUSER_ID, @CJRN_ID, @CACTION, @CCOMPANY_ID, " +
                        "@CDEPT_CODE, @CTRANS_CODE, @CREF_NO, @CDOC_NO, @CDOC_DATE " +
                        "@IFREQUENCY, @IPERIOD, @CSTART_DATE, @CTRANS_DESC, @CCURRENCY_CODE, @LFIX_RATE " +
                        "@NLBASE_RATE, @NLCURRENCY_RATE, @NBBASE_RATE, @NBCURRENCY_RATE, @NPRELIST_AMOUNT " +
                        " ";
                    loCmd.CommandText = lcQuery;
                    loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, poNewEntity.CUSER_ID.Length, poNewEntity.CUSER_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CJRN_ID", DbType.String, poNewEntity.CJRN_ID.Length, poNewEntity.CJRN_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, poNewEntity.CACTION.Length, poNewEntity.CACTION);
                    loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, poNewEntity.CCOMPANY_ID.Length, poNewEntity.CCOMPANY_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, poNewEntity.CDEPT_CODE.Length, poNewEntity.CDEPT_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, poNewEntity.CTRANS_CODE.Length, poNewEntity.CTRANS_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, poNewEntity.CREF_NO.Length, poNewEntity.CREF_NO);
                    loDB.R_AddCommandParameter(loCmd, "@CDOC_NO", DbType.String, poNewEntity.CDOC_NO.Length, poNewEntity.CDOC_NO);
                    loDB.R_AddCommandParameter(loCmd, "@CDOC_DATE", DbType.String, poNewEntity.CDOC_DATE.Length, poNewEntity.CDOC_DATE);
                    loDB.R_AddCommandParameter(loCmd, "@IFREQUENCY", DbType.Int32, 2, poNewEntity.IFREQUENCY);
                    loDB.R_AddCommandParameter(loCmd, "@IPERIOD", DbType.Int32, 2, poNewEntity.IPERIOD);
                    loDB.R_AddCommandParameter(loCmd, "@CSTART_DATE", DbType.String, poNewEntity.CSTART_DATE.Length, poNewEntity.CSTART_DATE);
                    loDB.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, poNewEntity.CTRANS_DESC.Length, poNewEntity.CTRANS_DESC);
                    loDB.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, poNewEntity.CCURRENCY_CODE.Length, poNewEntity.CCURRENCY_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@LFIX_RATE", DbType.Boolean, 2, poNewEntity.LFIX_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, Decimal.MaxValue.ToString().Length, poNewEntity.NLBASE_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, Decimal.MaxValue.ToString().Length, poNewEntity.NLCURRENCY_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, Decimal.MaxValue.ToString().Length, poNewEntity.NBBASE_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, Decimal.MaxValue.ToString().Length, poNewEntity.NBCURRENCY_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NPRELIST_AMOUNT", DbType.Decimal, Decimal.MaxValue.ToString().Length, poNewEntity.NPRELIST_AMOUNT);

                    try
                    {
                        loDB.SqlExecNonQuery(loConn, loCmd, false);
                    }
                    catch (Exception ex)
                    {
                        loEx.Add(ex);
                    }
                    loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));

                    TransScope.Complete();
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }
                    loConn.Dispose();
                }
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        public List<JournalGridDTO> GetJournalList(RecurringJournalListParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            List<JournalGridDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GL_SEARCH_RECURRING_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParam.CUSER_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, 20, poParam.CTRANS_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 20, poParam.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CPERIOD", DbType.String, 6, poParam.CPERIOD_YYYYMM);
                loDB.R_AddCommandParameter(loCmd, "@CSTATUS", DbType.String, 2, poParam.CSTATUS);
                loDB.R_AddCommandParameter(loCmd, "@CSEARCH_TEXT", DbType.String, 20, poParam.CSEARCH_TEXT);
                loDB.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 20, poParam.CDEPT_CODE);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<JournalGridDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;

        }
        public List<JournalDetailGridDTO> GetJournalDetailList(RecurringJournalListParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            List<JournalDetailGridDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GL_GET_RECURRING_DETAIL_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CJRN_ID", DbType.String, 50, poParam.CREC_ID);
                loDB.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, poParam.CLANGUAGE_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<JournalDetailGridDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;

        }
        public REFRESH_CURRENCY_RATE_RESULT RefreshCurrencyRate(REFRESH_CURRENCY_RATE_PARAM poParam)
        {
            R_Exception loEx = new R_Exception();
            REFRESH_CURRENCY_RATE_RESULT loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_CURRENCY_RATE";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, poParam.CCURRENCY_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CRATETYPE_CODE", DbType.String, 50, poParam.CRATETYPE_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CRATE_DATE", DbType.String, 50, poParam.CSTART_DATE);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<REFRESH_CURRENCY_RATE_RESULT>(loRtnTemp).FirstOrDefault();

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public List<JournalDetailActualGridDTO> GetActualJournalDetailList(RecurringJournalListParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            List<JournalDetailActualGridDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GL_GET_RECURRING_ACTUAL_JRN_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poParam.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, 50, poParam.CREF_NO);
                loDB.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, poParam.CLANGUAGE_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<JournalDetailActualGridDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;

        }

        #region Init var
        //public INIT_VAR_RESULT GetInitData(INIT_VAR_DB_PARAM poParam)
        //{
        //    R_Exception loEx = new R_Exception();
        //    INIT_VAR_RESULT loRtn = null;
        //    try
        //    {
        //        using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.RequiresNew))
        //        {
        //            loRtn = new INIT_VAR_RESULT()
        //            {
        //            };

        //            TransScope.Complete();
        //            loRtn.IsSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);

        //    }
        //    loEx.ThrowExceptionIfErrors();
        //    return loRtn;
        //}
        public VAR_GSM_COMPANY_DTO GetVAR_GSM_COMPANY(INIT_VAR_DB_PARAM poParam)
        {

            R_Exception loException = new R_Exception();
            VAR_GSM_COMPANY_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT CCOGS_METHOD,LENABLE_CENTER_IS,LENABLE_CENTER_BS,LPRIMARY_ACCOUNT,CBASE_CURRENCY_CODE ,CLOCAL_CURRENCY_CODE FROM GSM_COMPANY (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_COMPANY_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;

        }
        public VAR_GL_SYSTEM_PARAM_DTO GetVAR_GL_SYSTEM_PARAM(INIT_VAR_DB_PARAM poParam)
        {

            R_Exception loException = new R_Exception();
            VAR_GL_SYSTEM_PARAM_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "EXEC RSP_GL_GET_SYSTEM_PARAM @CCOMPANY_ID,''";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_GL_SYSTEM_PARAM_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;

        }
        public VAR_CCURRENT_PERIOD_START_DATE_DTO GetCCURRENT_PERIOD_START_DATE(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            VAR_CCURRENT_PERIOD_START_DATE_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT CSTART_DATE FROM GSM_PERIOD_DT (NOLOCK) WHERE CCOMPANY_ID=@CCOMPANY_ID AND CCYEAR=@CCURRENT_PERIOD_YY AND CPERIOD_NO= @CCURRENT_PERIOD_MM";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENT_PERIOD_YY", DbType.String, 50, poParam.CCURRENT_PERIOD_YY);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENT_PERIOD_MM", DbType.String, 50, poParam.CCURRENT_PERIOD_MM);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_CCURRENT_PERIOD_START_DATE_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public VAR_CSOFT_PERIOD_START_DATE_DTO GetCSOFT_PERIOD_START_DATE(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            VAR_CSOFT_PERIOD_START_DATE_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT CSTART_DATE FROM GSM_PERIOD_DT (NOLOCK) WHERE CCOMPANY_ID=@CCOMPANY_ID AND CCYEAR=@CSOFT_PERIOD_YY AND CPERIOD_NO= @CSOFT_PERIOD_MM";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CSOFT_PERIOD_YY", DbType.String, 50, poParam.CSOFT_PERIOD_YY);
                loDb.R_AddCommandParameter(loCmd, "@CSOFT_PERIOD_MM", DbType.String, 50, poParam.CSOFT_PERIOD_MM);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_CSOFT_PERIOD_START_DATE_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public VAR_IUNDO_COMMIT_JRN_DTO GetIUNDO_COMMIT_JRN(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            VAR_IUNDO_COMMIT_JRN_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT IOPTION FROM GLM_SYSTEM_ENABLE_OPTION (NOLOCK) WHERE CCOMPANY_ID=@CCOMPANY_ID AND COPTION_CODE='GL014001'";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_IUNDO_COMMIT_JRN_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public VAR_GSM_TRANSACTION_CODE_DTO GetGSM_TRANSACTION_CODE(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            VAR_GSM_TRANSACTION_CODE_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT LINCREMENT_FLAG LAPPROVAL_FLAG FROM GSM_TRANSACTION_CODE (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID AND CTRANSACTION_CODE='000000'";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_TRANSACTION_CODE_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public VAR_GSM_PERIOD_DTO GetGSM_PERIOD(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            VAR_GSM_PERIOD_DTO loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT IMIN_YEAR=CAST(MIN(CYEAR) AS INT),IMAX_YEAR=CAST(MAX(CYEAR) AS INT) FROM GSM_PERIOD (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_PERIOD_DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public List<VAR_STATUS_DTO> GetSTATUS_DTO(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loException = new R_Exception();
            List<VAR_STATUS_DTO> loResult = null;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = "SELECT CCODE='',CNAME='All' UNION SELECT CCODE,CDESCRIPTION AS CNAME FROM RFT_GET_GSB_CODE_INFO('BIMASAKTI', @CCOMPANY_ID, '_GL_JOURNAL_STATUS', '', @CLANGUAGE_ID) ORDER BY CCODE";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, poParam.CLANGUAGE_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_STATUS_DTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        public List<VAR_CURRENCY> GetCurrency(INIT_VAR_DB_PARAM poParam)
        {
            R_Exception loEx = new R_Exception();
            List<VAR_CURRENCY> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_CURRENCY_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParam.CUSER_ID);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<VAR_CURRENCY>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        #endregion Init Var

        #region Commit/Approve
        public void CommitApproveJournal(JournalCommitApprovalPARAM poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbCommand loCmd;
            DbConnection loConn;
            string lcQuery = "";
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();
                lcQuery = "RSP_GL_UPDATE_RECURRING_STATUS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDB.R_AddCommandParameter(loCmd, "@CAPPROVE_BY", DbType.String, 50, poParameter.CAPPROVE_BY);
                loDB.R_AddCommandParameter(loCmd, "@CJRN_ID_LIST", DbType.String, 50, poParameter.CJRN_ID_LIST);
                loDB.R_AddCommandParameter(loCmd, "@CNEW_STATUS", DbType.String, 50, poParameter.CNEW_STATUS);
                loDB.R_AddCommandParameter(loCmd, "@LAUTO_COMMIT", DbType.Boolean, 50, poParameter.LAUTO_COMMIT);
                loDB.R_AddCommandParameter(loCmd, "@LUNDO_COMMIT", DbType.Boolean, 50, poParameter.LUNDO_COMMIT);
                loDB.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
    }
}
