using LMM03700Common.DTO_s;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;
using R_APICommonDTO;
using System.Windows.Input;
using System.Transactions;
using RSP_LM_MAINTAIN_TENANT_CLASSResources;
using RSP_LM_MAINTAIN_TENANT_CLASS_GRPResources;

namespace LMM03700Back
{
    public class LMM03710Cls : R_BusinessObject<TenantClassificationDTO>
    {

        RSP_LM_MAINTAIN_TENANT_CLASSResources.Resources_Dummy_Class _rspTenantClass = new();
        RSP_LM_MAINTAIN_TENANT_CLASS_GRPResources.Resources_Dummy_Class _rspTenantClassGRP = new();

        protected override void R_Deleting(TenantClassificationDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            TenantClassificationDTO loRtn = null;
            R_Db loDB;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);
                lcQuery = "RSP_LM_MAINTAIN_TENANT_CLASS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, 50, poEntity.CTENANT_CLASSIFICATION_GROUP_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 50, poEntity.CTENANT_CLASSIFICATION_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_NAME", DbType.String, 50, poEntity.CTENANT_CLASSIFICATION_NAME);
                loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 50, "DELETE");
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                try
                {
                    loDB.SqlExecNonQuery(loConn, loCmd, false);
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

        protected override TenantClassificationDTO R_Display(TenantClassificationDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            TenantClassificationDTO loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_TENANT_CLASS_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, poEntity.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, 20, poEntity.CTENANT_CLASSIFICATION_GROUP_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 20, poEntity.CTENANT_CLASSIFICATION_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 80, poEntity.CUSER_ID);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<TenantClassificationDTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(TenantClassificationDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                R_ExternalException.R_SP_Init_Exception(loConn);

                switch (poCRUDMode)
                {
                    case eCRUDMode.AddMode:
                        lcAction = "ADD";
                        break;

                    case eCRUDMode.EditMode:
                        lcAction = "EDIT";
                        break;
                }

                string lcQuery = "RSP_LM_MAINTAIN_TENANT_CLASS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, poNewEntity.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, 20, poNewEntity.CTENANT_CLASSIFICATION_GROUP_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 20, poNewEntity.CTENANT_CLASSIFICATION_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_NAME", DbType.String, 100, poNewEntity.CTENANT_CLASSIFICATION_NAME);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poNewEntity.CUSER_ID);

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

        public List<TenantClassificationDTO> GetTenantClassList(TenantClassificationDBListMaintainParamDTO loParam)
        {
            List<TenantClassificationDTO> loRtn = new List<TenantClassificationDTO>();
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_TENANT_CLASS_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, loParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, loParam.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, 20, loParam.CTENANT_CLASSIFICATION_GROUP_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, loParam.CUSER_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<TenantClassificationDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public List<TenantDTO> GetAssignedTenantList(TenantClassificationDBListMaintainParamDTO loParam)
        {
            List<TenantDTO> loRtn = new List<TenantDTO>();
            R_Exception loEx = new R_Exception();
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_LM_GET_TENANT_CLASS_TENANT_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, loParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, loParam.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 20, loParam.CTENANT_CLASSIFICATION_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, loParam.CUSER_ID);
                var loRtnTemp = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<TenantDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        #region AssignTenant
        public List<TenantDTO> GetTenantToAssigntList(TenantClassificationDBListMaintainParamDTO loParam)
        {
            List<TenantDTO> loRtn = new List<TenantDTO>();
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_ASSIGN_TENANT_CLASS_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, loParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, loParam.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 20, loParam.CTENANT_CLASSIFICATION_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, 20, loParam.CTENANT_CLASSIFICATION_GROUP_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, loParam.CUSER_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<TenantDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public void AssignTenant(TenantParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                R_ExternalException.R_SP_Init_Exception(loConn);

                string lcQuery = "RSP_LM_ASSIGN_TENANT_CLASS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, Int32.MaxValue, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, Int32.MaxValue, poParam.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, Int32.MaxValue, poParam.CTENANT_CLASSIFICATION_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, Int32.MaxValue, poParam.CTENANT_CLASSIFICATION_GROUP_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_LIST", DbType.String, Int32.MaxValue, poParam.CTENANT_ID_LIST_COMMA_SEPARATOR);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, Int32.MaxValue, poParam.CUSER_ID);

                try
                {
                    var loResult = loDb.SqlExecQuery(loConn, loCmd, false);
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
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region MoveTenant
        public List<TenantDTO> GetTenantToMoveList(TenantParamDTO loParam)
        {
            List<TenantDTO> loRtn = new List<TenantDTO>();
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_TENANT_LIST_CLASS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, loParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, loParam.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_ID", DbType.String, 50, loParam.CTENANT_CLASSIFICATION_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, loParam.CUSER_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<TenantDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public void MoveTenant(TenantMoveParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);
                string lcQuery = "RSP_LM_MOVE_TENANT_CLASS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, Int32.MaxValue, poParam.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, Int32.MaxValue, poParam.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_CLASSIFICATION_GROUP_ID", DbType.String, Int32.MaxValue, poParam.CTENANT_CLASSIFICATION_GROUP_ID);
                loDb.R_AddCommandParameter(loCmd, "@CFROM_TENANT_CLASSIFICATION_ID", DbType.String, Int32.MaxValue, poParam.CFROM_TENANT_CLASSIFICATION_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTO_TENANT_CLASSIFICATION_ID", DbType.String, Int32.MaxValue, poParam.CTO_TENANT_CLASSIFICATION_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTENANT_LIST", DbType.String, Int32.MaxValue, poParam.CTENANT_ID_LIST_COMMA_SEPARATOR);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, Int32.MaxValue, poParam.CUSER_ID);

                try
                {
                    var loResult = loDb.SqlExecQuery(loConn, loCmd, false);
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
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion
    }
}