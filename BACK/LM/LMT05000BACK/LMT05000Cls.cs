﻿using LMT05000COMMON;
using LMT05000COMMON.DTO_s;
using R_BackEnd;
using R_Common;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using System.Transactions;

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

        //method start here
        public List<AgreementChrgDiscDetailDTO> GetAgreementChargesDiscountList( AgreementChrgDiscListParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            List<AgreementChrgDiscDetailDTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection();
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_LM_GET_AGREEMENT_CHARGES_DISC_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poParam.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCHARGES_TYPE", DbType.String, int.MaxValue, poParam.CCHARGES_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CCHARGES_ID", DbType.String, int.MaxValue, poParam.CCHARGES_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDISCOUNT_CODE", DbType.String, int.MaxValue, poParam.CDISCOUNT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDISCOUNT_TYPE", DbType.String, int.MaxValue, poParam.CDISCOUNT_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CINV_PRD", DbType.String, int.MaxValue, poParam.CINV_PRD);
                loDb.R_AddCommandParameter(loCmd, "@CINV_PRD", DbType.Boolean, 10, poParam.LALL_BUILDING);
                loDb.R_AddCommandParameter(loCmd, "@CBUILDING_ID", DbType.String, int.MaxValue, poParam.CBUILDING_ID);
                loDb.R_AddCommandParameter(loCmd, "@CAGREEMENT_TYPE", DbType.String, int.MaxValue, poParam.CAGREEMENT_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poParam.CUSER_ID);


                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<AgreementChrgDiscDetailDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public void ProcessAgreementChargeDiscount(AgreementChrgDiscParamDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd = null;
            AgreementChrgDiscParamDTO loRtn = poEntity;
            var loDetaildata = new List<AgreementChrgDiscDetailBulkDTO>();
            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //List<AgreementChrgDiscDetailBulkDTO> loDetailData = poEntity.AgreementChrgDiscDetail;
                    loConn = loDb.GetConnection("R_DefaultConnectionString");
                    loCmd = loDb.GetCommand();

                    //create temptable
                    lcQuery = @"CREATE TABLE #LEASE_PRICING
                        (	
	                      CCOMPANY_ID VARCHAR(8)
                        , CPROPERTY_ID VARCHAR(20)
                        , CREF_NO VARCHAR(30)
                        , CSEQ_NO VARCHAR(30)
                        , CFLOOR_ID VARCHAR(30)
                        , CUNIT_ID VARCHAR(30)
                        , CTENANT_ID VARCHAR(30)
                        , NCHARGES_AMOUNT NUMERIC(18,2)
                        , NCHARGE_DISCOUNT NUMERIC(18,2)
                        , NNET_CHARGE VARCHAR(30)
                        , CAGREEMENT_NO VARCHAR(30)
                        , CSTART_DATE VARCHAR(30)
                        , CEND_DATE VARCHAR(30)
                        , CEXISTING_DISCOUNT_CODE VARCHAR(30)
                        , NEXISTING_CHARGE_DISCOUNT VARCHAR(30)
                        )";
                    ShowLogDebug(lcQuery, loCmd.Parameters);//log create
                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    //logger
                    _logger.LogDebug($"INSERT INTO #LEASE_PRICING {loDetaildata}");//log insert
                    //copybulk
                    loDb.R_BulkInsert<AgreementChrgDiscDetailBulkDTO>((SqlConnection)loConn, "#LEASE_PRICING", loDetaildata);

                    //exec sp
                    lcQuery = "RSP_LM_PROCESS_AGREEMENT_CHARGE_DISCOUNT";
                    loCmd.CommandText = lcQuery;
                    loCmd.CommandType = CommandType.StoredProcedure;

                    loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poEntity.CCOMPANY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poEntity.CPROPERTY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, int.MaxValue, poEntity.CREF_NO);
                    loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String, int.MaxValue, poEntity.CREF_DATE);
                    loDb.R_AddCommandParameter(loCmd, "@CCHARGES_TYPE", DbType.String, int.MaxValue, poEntity.CCHARGES_TYPE);
                    loDb.R_AddCommandParameter(loCmd, "@CCHARGES_ID", DbType.String, int.MaxValue, poEntity.CCHARGES_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CDISCOUNT_CODE", DbType.String, int.MaxValue, poEntity.CDISCOUNT_CODE);
                    loDb.R_AddCommandParameter(loCmd, "@CINV_PERIOD_YEAR", DbType.String, int.MaxValue, poEntity.CINV_PERIOD_YEAR);
                    loDb.R_AddCommandParameter(loCmd, "@CINV_PERIOD_MONTH", DbType.String, int.MaxValue, poEntity.CINV_PERIOD_MONTH);
                    loDb.R_AddCommandParameter(loCmd, "@LALL_BUILDING", DbType.Boolean, 10, poEntity.LALL_BUILDING);
                    loDb.R_AddCommandParameter(loCmd, "@CBUILDING_ID", DbType.String, int.MaxValue, poEntity.CBUILDING_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, int.MaxValue, poEntity.CACTION);
                    loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poEntity.CUSER_ID);

                    R_ExternalException.R_SP_Init_Exception(loConn);

                    try
                    {
                        ShowLogDebug(lcQuery, loCmd.Parameters);
                        var loDataTable = loDb.SqlExecQuery(loConn, loCmd, false);

                        //var loTempResult = R_Utility.R_ConvertTo<ConvertRecID>(loDataTable).FirstOrDefault();
                        //loRtn.CREC_ID = loTempResult.CJRN_ID;
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
            //return loRtn;
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
