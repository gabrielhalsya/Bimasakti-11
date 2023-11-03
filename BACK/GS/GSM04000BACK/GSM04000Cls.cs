using GSM04000Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Input;

namespace GSM04000Back
{
    public class GSM04000Cls : R_BusinessObject<GSM04000DTO>
    {
        private LoggerGSM04000 _logger;

        public GSM04000Cls()
        {
            _logger = LoggerGSM04000.R_GetInstanceLogger();
        }

        protected override void R_Deleting(GSM04000DTO poEntity)
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

                lcQuery = "RSP_GS_MAINTAIN_DEPARTMENT";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 8, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_NAME", DbType.String, 80, poEntity.CDEPT_NAME);
                loDb.R_AddCommandParameter(loCmd, "@CCENTER_CODE", DbType.String, 8, poEntity.CCENTER_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CMANAGER_NAME", DbType.String, 8, poEntity.CMANAGER_NAME);
                loDb.R_AddCommandParameter(loCmd, "@LEVERYONE", DbType.Boolean, 2, poEntity.LEVERYONE);
                loDb.R_AddCommandParameter(loCmd, "@LACTIVE", DbType.Boolean, 2, poEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, "DELETE");
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poEntity.CUSER_ID);

                try
                {
                    var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE" or "@CDEPT_NAME" or "@CCENTER_CODE" or "@CMANAGER_NAME" or "@LEVERYONE" or "@LACTIVE" or "@CACTION" or "@CUSER_ID")
                    .Select(x => x.Value); //gettin param into var to show in log debug
                    _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    loEx.Add(ex);
                    _logger.LogError(loEx);
                }
                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
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

        protected override GSM04000DTO R_Display(GSM04000DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM04000DTO loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_DEPT_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);

                var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE" or "@CUSER_ID")
                    .Select(x => x.Value); //gettin param into var to show in log debug
                _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04000DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(GSM04000DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            string lcQuery;
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

                lcQuery = "RSP_GS_MAINTAIN_DEPARTMENT";

                switch (poCRUDMode)
                {
                    case eCRUDMode.AddMode:
                        lcAction = "ADD";
                        break;

                    case eCRUDMode.EditMode:
                        lcAction = "EDIT";
                        break;
                }

                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 8, poNewEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_NAME", DbType.String, 80, poNewEntity.CDEPT_NAME);
                loDb.R_AddCommandParameter(loCmd, "@CCENTER_CODE", DbType.String, 8, poNewEntity.CCENTER_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CMANAGER_NAME", DbType.String, 8, poNewEntity.CMANAGER_NAME);
                loDb.R_AddCommandParameter(loCmd, "@LEVERYONE", DbType.Boolean, 2, poNewEntity.LEVERYONE);
                loDb.R_AddCommandParameter(loCmd, "@LACTIVE", DbType.Boolean, 2, poNewEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poNewEntity.CUSER_ID);
                try
                {
                    var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE" or "@CDEPT_NAME" or "@CCENTER_CODE" or "@CMANAGER_NAME" or "@LEVERYONE" or "@LACTIVE" or "@CACTION" or "@CUSER_ID")
                    .Select(x => x.Value); //gettin param into var to show in log debug
                    _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
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
                _logger.LogError(loEx);
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

        public List<GSM04000DTO> GetDeptList(GSM04000ListDBParameterDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            List<GSM04000DTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_DEPT_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_LOGIN_ID);

                var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                .Where(x =>
                    x.ParameterName is
                        "@CCOMPANY_ID" or
                        "@CUSER_ID"
                )
                .Select(x => x.Value);

                _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04000DTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public void RSP_GS_ACTIVE_INACTIVE_DEPTMethodCls(GSM04000ActiveInactiveParam poEntity)
        {
            R_Exception loex = new R_Exception();
            string lcQuery = "";
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                lcQuery = "RSP_GS_ACTIVE_INACTIVE_DEPT";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 20, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@LACTIVE", DbType.Boolean, 2, poEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poEntity.CUSER_ID);

                var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE" or "@LACTIVE" or "@CUSER_ID")
                    .Select(x => x.Value); //gettin param into var to show in log debug
                _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                loex.Add(ex);
                _logger.LogError(loex);
            }
        EndBlock:
            loex.ThrowExceptionIfErrors();
        }

        public bool CheckIsUserDeptExist(GSM04000DTO poEntity)
        {
            R_Exception loException = new R_Exception();
            bool loRtn = true;
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = $"SELECT TOP 1 1 FROM GSM_DEPT_USER (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID AND CDEPT_CODE = @CDEPT_CODE";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);
                
                var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE").Select(x => x.Value); //gettin param into var to show in log debug
                _logger.LogDebug("EXEC {lcQuery} {@poParam}", lcQuery, loDbParam);
                
                var loResult = loDb.SqlExecQuery(loConn, loCmd, true);
                
                loRtn = loResult == null ? false : true;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

        public void DeleteAssignedUserDept(GSM04000DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            string lcCmd = "";
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                lcCmd = "DELETE GSM_DEPT_USER WHERE CCOMPANY_ID = @CCOMPANY_ID AND CDEPT_CODE = @CDEPT_CODE";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcCmd;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 8, poEntity.CDEPT_CODE);
                var loDbParam = loCmd.Parameters.Cast<DbParameter>().Where(x => x.ParameterName is "@CCOMPANY_ID" or "@CDEPT_CODE").Select(x => x.Value); //gettin param into var to show in log debug
                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
    }
}
