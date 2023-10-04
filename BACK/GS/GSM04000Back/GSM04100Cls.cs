using GSM04000Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM04000Back
{
    public class GSM04100Cls : R_BusinessObject<GSM04100DTO>
    {
        protected override void R_Deleting(GSM04100DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_MAINTAIN_DEPT_USER";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 50, "DELETE");
                loDB.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 50, poEntity.CUSER_LOGIN_ID);
                loDB.SqlExecNonQuery(loConn, loCmd, true);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        protected override GSM04100DTO R_Display(GSM04100DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM04100DTO loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_DEPT_USER_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04100DTO>(loRtnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(GSM04100DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_MAINTAIN_DEPT_USER";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poNewEntity.CDEPT_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poNewEntity.CUSER_ID);
                loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 50, "ADD");
                loDB.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 50, poNewEntity.CUSER_LOGIN_ID);

                loDB.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public List<GSM04100StreamDTO> GetUserDeptList(GSM04100ListDBParameterDTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            List<GSM04100StreamDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_DEPT_USER_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04100StreamDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public List<GSM04100DTO> GetUserToAssignList(GSM04100ListDBParameterDTO poParam)
        {
            List<GSM04100DTO> loRtn = null;
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GS_GET_LOOKUP_USER_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROGRAM_ID", DbType.String, 50, poParam.CPROGRAM_CODE);
                loDB.R_AddCommandParameter(loCmd, "@CPARAMETER_ID", DbType.String, 50, poParam.CDEPT_CODE);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04100DTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

    }
}
