﻿using GLT00100Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;

namespace GLT00100Back
{
    public class GLT00100Cls : R_BusinessObject<GLT00100DTO>
    {
        protected override void R_Deleting(GLT00100DTO poEntity)
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
                loDb.R_AddCommandParameter(loCmd, "@CJRN_ID_LIST", DbType.String, 2047483647, poEntity.CREC_ID);
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

        protected override GLT00100DTO R_Display(GLT00100DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GLT00100DTO loResult = null;
            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_GL_GET_JOURNAL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 100, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 100, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 3, poEntity.CLANGUAGE_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<GLT00100DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        protected override void R_Saving(GLT00100DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDB = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            string lcAction = "";
            List<GLT00100DTO> loResult = null;
            try
            {
                switch (poCRUDMode)
                {
                    case eCRUDMode.AddMode:
                        lcAction = "NEW";

                        break;
                    case eCRUDMode.EditMode:
                        lcAction = "EDIT";
                        break;
                }

                loCmd = loDB.GetCommand();
                loConn = loDB.GetConnection();

                var tempParam = R_Utility.R_ConvertCollectionToCollection<GLT00100DetailDTO, GLT00100DetailDTO>(poNewEntity
                    .LIST_JOURNAL_DETAIL);
                lcQuery = @"CREATE TABLE #GLT0100_JOURNAL_DETAIL 
                              ( CGLACCOUNT_NO VARCHAR(20)
                                ,CCENTER_CODE    VARCHAR(10)
                                ,CDBCR           CHAR(1)
                                ,NAMOUNT         NUMERIC(19,2)
                                ,CDETAIL_DESC    NVARCHAR(200)
                                ,CDOCUMENT_NO    VARCHAR(20)
                                ,CDOCUMENT_DATE  VARCHAR(8)
                              ) ";
                try
                {
                    R_ExternalException.R_SP_Init_Exception(loConn);
                    loDB.SqlExecNonQuery(lcQuery, loConn, false);
                    loDB.R_BulkInsert((SqlConnection)loConn, "#GLT0100_JOURNAL_DETAIL", tempParam);
                    //lcQuery = "select * from #GLT0100_JOURNAL_DETAIL ";

                    lcQuery = @"RSP_GL_SAVE_JOURNAL";
                    loCmd.CommandType = CommandType.StoredProcedure;
                    loCmd.CommandText = lcQuery;
                    loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poNewEntity.CUSER_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CJRN_ID", DbType.String, 50, poNewEntity.CREC_ID ?? "");
                    loDB.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 50, lcAction);
                    loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                    loDB.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poNewEntity.CDEPT_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, 50, poNewEntity.CTRANS_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, 50, poNewEntity.CREF_NO);
                    loDB.R_AddCommandParameter(loCmd, "@CDOC_NO", DbType.String, 50, poNewEntity.CDOC_NO);
                    loDB.R_AddCommandParameter(loCmd, "@CDOC_DATE", DbType.String, 50, poNewEntity.CDOC_DATE);
                    loDB.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String, 50, poNewEntity.CREF_DATE);
                    loDB.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, 50, poNewEntity.CTRANS_DESC);
                    loDB.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, poNewEntity.CCURRENCY_CODE);
                    loDB.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, 50, poNewEntity.NLBASE_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, 50, poNewEntity.NLCURRENCY_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, 50, poNewEntity.NBBASE_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, 50, poNewEntity.NBCURRENCY_RATE);
                    loDB.R_AddCommandParameter(loCmd, "@NPRELIST_AMOUNT", DbType.Decimal, 50, poNewEntity.NPRELIST_AMOUNT);
                    var loDataTable = loDB.SqlExecQuery(loConn, loCmd, false);
                    loResult = R_Utility.R_ConvertTo<GLT00100DTO>(loDataTable).ToList();

                    if (string.IsNullOrWhiteSpace(poNewEntity.CREC_ID) || string.IsNullOrEmpty(poNewEntity.CREC_ID))
                    {
                        poNewEntity.CREC_ID = loResult.FirstOrDefault().CREC_ID;

                    }
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

    }
}