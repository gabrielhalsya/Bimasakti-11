using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;
using LMM01500Common;
using System.Runtime.CompilerServices;
using System.Runtime;
using System.Windows.Input;

namespace LMM01500Back
{
    public class InvoiceGroupCls : R_BusinessObject<InvoiceGroupParamDTO>
    {
        private LMM01500Logger _Logger;

        public InvoiceGroupCls()
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

        protected override void R_Deleting(InvoiceGroupParamDTO poEntity)
        {
            var loException = new R_Exception();
            DbCommand loCommand = null;
            DbConnection loConn = null;
            InvoiceGroupParamDTO loDeleteParameter;
            try
            {
                var loDb = new R_Db();
                loConn = loDb.GetConnection();
                R_ExternalException.R_SP_Init_Exception(loConn);
                loCommand = loDb.GetCommand();
                string lcAction = "DELETE";


                //if (!String.IsNullOrEmpty(poEntity.CSTORAGE_ID))
                //{
                //    loDeleteParameter = new R_DeleteParameter()
                //    {
                //        StorageId = poEntity.CSTORAGE_ID,
                //        UserId = poEntity.CUSER_ID
                //    };
                //    R_StorageUtility.DeleteFile(loDeleteParameter, "R_DefaultConnectionString");
                //}

                var lcQuery = "RSP_LM_MAINTAIN_INVOICE_GRP";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 20, poEntity.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CINVGRP_CODE", DbType.String, 8, poEntity.CINVGRP_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CINVGRP_NAME", DbType.String, 100, poEntity.CINVGRP_NAME);
                loDb.R_AddCommandParameter(loCommand, "@CSEQUENCE", DbType.String, 6, poEntity.CSEQUENCE);

                loDb.R_AddCommandParameter(loCommand, "@LACTIVE", DbType.Boolean, 2, poEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCommand, "@CINVOICE_DUE_MODE", DbType.String, 2, poEntity.CINVOICE_DUE_MODE);
                loDb.R_AddCommandParameter(loCommand, "@CINVOICE_GROUP_MODE", DbType.String, 2, poEntity.CINVOICE_GROUP_MODE);

                loDb.R_AddCommandParameter(loCommand, "@IDUE_DAYS ", DbType.Int32, 512, poEntity.IDUE_DAYS);
                loDb.R_AddCommandParameter(loCommand, "@IFIXED_DUE_DATE", DbType.Int32, 512, poEntity.IFIXED_DUE_DATE);
                loDb.R_AddCommandParameter(loCommand, "@ILIMIT_INVOICE_DATE ", DbType.Int32, 512, poEntity.ILIMIT_INVOICE_DATE);
                loDb.R_AddCommandParameter(loCommand, "@IBEFORE_LIMIT_INVOICE_DATE ", DbType.Int32, 512, poEntity.IBEFORE_LIMIT_INVOICE_DATE);
                loDb.R_AddCommandParameter(loCommand, "@IAFTER_LIMIT_INVOICE_DATE", DbType.Int32, 512, poEntity.IAFTER_LIMIT_INVOICE_DATE);

                loDb.R_AddCommandParameter(loCommand, "@LDUE_DATE_TOLERANCE_HOLIDAY", DbType.Boolean, 2, poEntity.LDUE_DATE_TOLERANCE_HOLIDAY);
                loDb.R_AddCommandParameter(loCommand, "@LDUE_DATE_TOLERANCE_SATURDAY", DbType.Boolean, 2, poEntity.LDUE_DATE_TOLERANCE_SATURDAY);
                loDb.R_AddCommandParameter(loCommand, "@LDUE_DATE_TOLERANCE_SUNDAY", DbType.Boolean, 2, poEntity.LDUE_DATE_TOLERANCE_SUNDAY);
                loDb.R_AddCommandParameter(loCommand, "LUSE_STAMP", DbType.Boolean, 2, poEntity.LUSE_STAMP);

                loDb.R_AddCommandParameter(loCommand, "@CSTAMP_ADD_ID", DbType.String, 20, poEntity.CSTAMP_ADD_ID);
                loDb.R_AddCommandParameter(loCommand, "@CDESCRIPTION ", DbType.Int32, Int32.MaxValue, poEntity.CDESCRIPTION);

                loDb.R_AddCommandParameter(loCommand, "@LBY_DEPARTMENT", DbType.Boolean, 2, poEntity.LBY_DEPARTMENT);

                loDb.R_AddCommandParameter(loCommand, "@CINVOICE_TEMPLATE", DbType.String, 100, poEntity.CINVOICE_TEMPLATE);
                loDb.R_AddCommandParameter(loCommand, "@CDEPT_CODE", DbType.String, 8, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CBANK_CODE", DbType.String, 8, poEntity.CBANK_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CBANK_ACCOUNT", DbType.String, 20, poEntity.CBANK_ACCOUNT);
                loDb.R_AddCommandParameter(loCommand, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 8, poEntity.CUSER_ID);
                LogDebug(loCommand);
                try
                {
                    loDb.SqlExecNonQuery(loConn, loCommand, false);
                }
                catch (Exception ex)
                {
                    loException.Add(ex);
                }

                loException.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                LogError(ex);
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
                if (loCommand != null)
                {
                    loCommand.Dispose();
                    loCommand = null;
                }
            }
            loException.ThrowExceptionIfErrors();
        }

        protected override InvoiceGroupParamDTO R_Display(InvoiceGroupParamDTO poEntity)
        {
            R_Exception loException = new R_Exception();
            InvoiceGroupParamDTO loReturn = null;
            R_Db loDb;
            try
            {
                var lcQuery = "RSP_LM_GET_INVOICE_GROUP";
                loDb = new R_Db();
                var loCommand = loDb.GetCommand();
                var loConn = loDb.GetConnection();
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;
                //param
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 10, poEntity.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCommand, "@CINVGRP_CODE", DbType.String, 20, poEntity.CINVGRP_CODE);

                //log
                LogDebug(loCommand);
                //exec
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, false);
                loReturn = R_Utility.R_ConvertTo<InvoiceGroupParamDTO>(loReturnTemp).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                LogError(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loReturn;
        }

        protected override void R_Saving(InvoiceGroupParamDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        public List<InvoiceGroupDTO> GetInvoiceGroupList(InvoiceGroupParamDTO poParam)
        {
            R_Exception loEx = new R_Exception();
            List<InvoiceGroupDTO> loRtn = new List<InvoiceGroupDTO>();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection("R_DefaultConnectionString");
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_LM_GET_INVOICE_GROUP_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParam.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poParam.CPROPERTY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParam.CUSER_ID);
                //log
                LogDebug(loCmd);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<InvoiceGroupDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                LogError(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
    }
}
