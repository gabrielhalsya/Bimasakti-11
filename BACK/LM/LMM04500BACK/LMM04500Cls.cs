using LMM04500COMMON;
using LMM04500COMMON.DTO_s;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace LMM04500BACK
{
    public class LMM04500Cls : R_BusinessObject<PricingParamDTO>
    {

        private LoggerLMM04500 _logger;

        private readonly ActivitySource _activitySource;

        public LMM04500Cls()
        {
            _logger = LoggerLMM04500.R_GetInstanceLogger();
            _activitySource = LMM04500Activity.R_GetInstanceActivitySource();
        }
        public List<PropertyDTO> GetPropertyList(PropertyDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new R_Exception();
            List<PropertyDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_PROPERTY_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poEntity.CUSER_ID);

                ShowLogDebug(lcQuery, loCmd.Parameters);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<PropertyDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public List<UnitTypeCategoryDTO> GetUnitTypeCategoryList(UnitTypeCategoryParamDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new();
            List<UnitTypeCategoryDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_UNIT_TYPE_CTG_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poEntity.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poEntity.CUSER_ID);
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<UnitTypeCategoryDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public List<PricingDTO> GetPricingList(PricingParamDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new();
            List<PricingDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_TENANT_CLASS_GRP_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poEntity.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUNIT_CATEGORY_ID", DbType.String, int.MaxValue, poEntity.CUNIT_TYPE_CATEGORY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPRICE_TYPE", DbType.String, int.MaxValue, poEntity.CPRICE_TYPE);
                loDB.R_AddCommandParameter(loCmd, "@LACTIVE_ONLY", DbType.Boolean, int.MaxValue, poEntity.CTYPE);
                loDB.R_AddCommandParameter(loCmd, "@CTYPE", DbType.String, int.MaxValue, poEntity.CTYPE);
                loDB.R_AddCommandParameter(loCmd, "@CVALID_DATE", DbType.String, int.MaxValue, poEntity.CVALID_DATE);
                loDB.R_AddCommandParameter(loCmd, "@CVALID_ID", DbType.String, int.MaxValue, poEntity.CVALID_INTERNAL_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poEntity.CUSER_ID);

                ShowLogDebug(lcQuery, loCmd.Parameters);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<PricingDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public List<PricingDTO> GetPricingDateList(PricingParamDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new();
            List<PricingDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_PRICING_DATE_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poEntity.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUNIT_CATEGORY_ID", DbType.String, int.MaxValue, poEntity.CUNIT_TYPE_CATEGORY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPRICE_TYPE", DbType.String, int.MaxValue, poEntity.CPRICE_TYPE);
                loDB.R_AddCommandParameter(loCmd, "@LACTIVE_ONLY", DbType.Boolean, int.MaxValue, poEntity.CTYPE);
                loDB.R_AddCommandParameter(loCmd, "@CTYPE", DbType.String, int.MaxValue, poEntity.CTYPE);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poEntity.CUSER_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<PricingDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public void SavePricing(PricingSaveParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                loCmd = loDB.GetCommand();
                loConn = loDB.GetConnection();
                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    // creating temptable
                    lcQuery = @"CREATE TABLE #LEASE_PRICING 
                              ( ISEQ INT
	                        , CVALID_INTERNAL_ID	VARCHAR(36)
	                        , CCHARGES_TYPE			VARCHAR(2)
	                        , CCHARGES_ID			VARCHAR(20)
	                        , CPRICE_MODE			VARCHAR(2)
	                        , NNORMAL_PRICE			NUMERIC(18,2)
	                        , NBOTTOM_PRICE			NUMERIC(18,2)
	                        , LOVERWRITE			BIT
                              ) ";
                    loDB.SqlExecNonQuery(lcQuery, loConn, false);

                    loDB.R_BulkInsert<PricingBulkSaveDTO>((SqlConnection)loConn, "#LEASE_PRICING", poParam.PRICING_LIST);

                    lcQuery = "RSP_LM_MAINTAIN_PRICING";
                    loCmd.CommandText = lcQuery;
                    loCmd.CommandType = CommandType.StoredProcedure;
                    loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poParam.CCOMPANY_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, int.MaxValue, poParam.CPROPERTY_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CPRICE_TYPE", DbType.String, int.MaxValue, poParam.CPRICE_TYPE);
                    loDB.R_AddCommandParameter(loCmd, "@CUNIT_TYPE_CTG_ID", DbType.String, int.MaxValue, poParam.CUNIT_TYPE_CATEGORY_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CVALID_FROM_DATE", DbType.String, int.MaxValue, poParam.CVALID_FROM_DATE);
                    loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, int.MaxValue, "ADD");
                    loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poParam.CUSER_ID);
                    var loDataTable = loDB.SqlExecQuery(loConn, loCmd, false);
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

        protected override void R_Deleting(PricingParamDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override PricingParamDTO R_Display(PricingParamDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(PricingParamDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
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
