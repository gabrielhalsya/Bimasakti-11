using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLT00100Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Reflection.Metadata;
using GLT00100Common;
using System.Windows.Input;
using System.Transactions;

namespace GLT00100Back
{
    public class GLT00100Cls : R_BusinessObject<GLT00100DTO>
    {
        protected override GLT00100DTO R_Display(GLT00100DTO poEntity)
        {
            var loEx = new R_Exception();
            GLT00100DTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = "EXEC RSP_GL_GET_JOURNAL @CCOMPANY_ID,@CUSER_ID,@CREC_ID,@CLANGUAGE_ID";
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
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    lcAction = "NEW";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    lcAction = "EDIT";
                }
                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loCmd = loDB.GetCommand();
                    loConn = loDB.GetConnection();

                    var tempParam = R_Utility.R_ConvertCollectionToCollection<GLT00100JournalGridDetailDTO, SaveJournalDetailDTO>(poNewEntity
                        .DetailList);
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


                        lcQuery = @"EXEC RSP_GL_SAVE_JOURNAL 
                              @CUSER_ID, @CJRN_ID, @CACTION, @CCOMPANY_ID, 
                              @CDEPT_CODE, @CTRANS_CODE, @CREF_NO, @CDOC_NO, @CDOC_DATE,@CREF_DATE,
                              @CREVERSE_DATE, @LREVERSE, @CTRANS_DESC, @CCURRENCY_CODE, 
                              @NLBASE_RATE, @NLCURRENCY_RATE, @NBBASE_RATE, @NBCURRENCY_RATE, @NPRELIST_AMOUNT 
                              ";
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
                        loDB.R_AddCommandParameter(loCmd, "@CREVERSE_DATE", DbType.String, 50, poNewEntity.CREVERSE_DATE);
                        loDB.R_AddCommandParameter(loCmd, "@LREVERSE", DbType.Boolean, 50, poNewEntity.LREVERSE);
                        loDB.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, 50, poNewEntity.CTRANS_DESC);
                        loDB.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, poNewEntity.CCURRENCY_CODE);
                        loDB.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, 50, poNewEntity.NLBASE_RATE);
                        loDB.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, 50, poNewEntity.NLCURRENCY_RATE);
                        loDB.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, 50, poNewEntity.NBBASE_RATE);
                        loDB.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, 50, poNewEntity.NBCURRENCY_RATE);
                        loDB.R_AddCommandParameter(loCmd, "@NPRELIST_AMOUNT", DbType.Decimal, 50, poNewEntity.NPRELIST_AMOUNT);


                        //loDB.SqlExecNonQuery(loConn, loCmd, false);

                        var loDataTable = loDB.SqlExecQuery(loConn, loCmd, false);
                        loResult = R_Utility.R_ConvertTo<GLT00100DTO>(loDataTable).ToList();

                        if (string.IsNullOrWhiteSpace(poNewEntity.CREC_ID))
                        {
                            poNewEntity.CREC_ID = loResult.FirstOrDefault().CJRN_ID;

                        }

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
        protected override void R_Deleting(GLT00100DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GLT00100JournalGridDTO> GetJournalList(GLT00100ParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            List<GLT00100JournalGridDTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_GL_SEARCH_REVERSING_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 20, poParameter.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CSEARCH_TEXT", DbType.String, 20, poParameter.CSEARCH_TEXT);
                loDb.R_AddCommandParameter(loCmd, "@CSTATUS", DbType.String, 20, poParameter.CSTATUS);
                loDb.R_AddCommandParameter(loCmd, "@CPERIOD", DbType.String, 20, poParameter.CPERIOD);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 20, poParameter.CLANGUAGE_ID);



                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<GLT00100JournalGridDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        public List<GLT00100JournalGridDetailDTO> GetJournalDetailList(GLT00100ParameterDTO poParameter)
        {
            R_Exception loEx = new R_Exception();
            List<GLT00100JournalGridDetailDTO> loRtn = null;
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = "RSP_GL_GET_JOURNAL_DETAIL_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CJRN_ID", DbType.String, 50, poParameter.CREC_ID);
                loDB.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, poParameter.CLANGUAGE_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GLT00100JournalGridDetailDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public void ProcessReversing(GLT00100ParameterDTO poParameter, GLT00100JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GL_PROCESS_REVERSING_JRN";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poData.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, 50, poData.CREF_NO);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 50, poData.CREC_ID);

                loDb.SqlExecNonQuery(loConn, loCmd, true);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public void ProcessJournalStatus(GLT00100ParameterDTO poParameter, GLT00100JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GL_UPDATE_JOURNAL_STATUS";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CAPPROVE_BY", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJRN_ID_LIST", DbType.String, 50, poData.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CNEW_STATUS", DbType.String, 50, poData.CSTATUS);
                loDb.R_AddCommandParameter(loCmd, "@LAUTO_COMMIT", DbType.Boolean, 50, poData.LCOMMIT_APRJRN);
                loDb.R_AddCommandParameter(loCmd, "@LUNDO_COMMIT", DbType.Boolean, 50, poData.LUNDO_COMMIT);

                loDb.SqlExecNonQuery(loConn, loCmd, true);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public void UndoReversingJournal(GLT00100ParameterDTO poParameter, GLT00100JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            DbCommand loCmd;
            string lcQuery = "";
            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GL_UNDO_REVERSING_JRN";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 50, poData.CREC_ID);

                loDb.SqlExecNonQuery(loConn, loCmd, true);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region init
        public VAR_GSM_COMPANYDTO GetJournalCompany(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            VAR_GSM_COMPANYDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @"SELECT CCOGS_METHOD,LENABLE_CENTER_IS,LENABLE_CENTER_BS,LPRIMARY_ACCOUNT,CBASE_CURRENCY_CODE ,CLOCAL_CURRENCY_CODE FROM GSM_COMPANY (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_COMPANYDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public VAR_GL_SYSTEM_PARAMDTO GetSystemParam(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            VAR_GL_SYSTEM_PARAMDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"exec RSP_GL_GET_SYSTEM_PARAM  @CCOMPANY_ID,@CLANGUAGE_ID ";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CLANGUAGE_ID", DbType.String, 50, poParameter.CLANGUAGE_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_GL_SYSTEM_PARAMDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public List<VAR_USER_DEPARTMENTDTO> GetDeptList(GLT00100ParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            List<VAR_USER_DEPARTMENTDTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = $"exec RSP_GS_GET_DEPT_LOOKUP_LIST'{poParameter.CCOMPANY_ID}', '{poParameter.CUSER_ID}'";
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<VAR_USER_DEPARTMENTDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        public VAR_CCURRENT_PERIOD_START_DATEDTO GetCurrentPeriodStartDate(GLT00100ParameterDTO poParameter, VAR_GL_SYSTEM_PARAMDTO poData)
        {
            R_Exception loException = new R_Exception();
            VAR_CCURRENT_PERIOD_START_DATEDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"SELECT CSTART_DATE FROM GSM_PERIOD_DT (NOLOCK)WHERE CCOMPANY_ID=@CCOMPANY_ID AND CCYEAR=@CCURRENT_PERIOD_YY AND CPERIOD_NO=@CCURRENT_PERIOD_MM";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CCURRENT_PERIOD_YY", DbType.String, 50, poData.CCURRENT_PERIOD_YY);
                loDb.R_AddCommandParameter(loCommand, "@CCURRENT_PERIOD_MM", DbType.String, 50, poData.CCURRENT_PERIOD_MM);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_CCURRENT_PERIOD_START_DATEDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public VAR_CSOFT_PERIOD_START_DATEDTO GetSoftPeriodStartDate(GLT00100ParameterDTO poParameter, VAR_GL_SYSTEM_PARAMDTO poData)
        {
            R_Exception loException = new R_Exception();
            VAR_CSOFT_PERIOD_START_DATEDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"SELECT CSTART_DATE FROM GSM_PERIOD_DT (NOLOCK)WHERE CCOMPANY_ID=@CCOMPANY_ID AND CCYEAR=@CSOFT_PERIOD_YY AND CPERIOD_NO=@CSOFT_PERIOD_MM";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CSOFT_PERIOD_YY", DbType.String, 50, poData.CSOFT_PERIOD_YY);
                loDb.R_AddCommandParameter(loCommand, "@CSOFT_PERIOD_MM", DbType.String, 50, poData.CSOFT_PERIOD_MM);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_CSOFT_PERIOD_START_DATEDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public VAR_IUNDO_COMMIT_JRNDTO GetIOption(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            VAR_IUNDO_COMMIT_JRNDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"SELECT IOPTION FROM GLM_SYSTEM_ENABLE_OPTION (NOLOCK) WHERE CCOMPANY_ID=@CCOMPANY_ID AND COPTION_CODE='GL014001'";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_IUNDO_COMMIT_JRNDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public VAR_GSM_TRANSACTION_CODEDTO GetLincementLapproval(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            VAR_GSM_TRANSACTION_CODEDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"SELECT LINCREMENT_FLAG ,LAPPROVAL_FLAG FROM GSM_TRANSACTION_CODE (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID AND CTRANS_CODE='000020'";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_TRANSACTION_CODEDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public VAR_GSM_PERIODDTO GetPeriod(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            VAR_GSM_PERIODDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @$"SELECT IMIN_YEAR=CAST(MIN(CYEAR) AS INT),IMAX_YEAR=CAST(MAX(CYEAR) AS INT) FROM GSM_PERIOD (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<VAR_GSM_PERIODDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        public List<StatusDTO> GetStatus(GLT00100ParameterDTO poParameter)
        {
            R_Exception loEx = new R_Exception();
            List<StatusDTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = $"SELECT CCODE='',CNAME='All' UNION SELECT CCODE,CDESCRIPTION AS CNAME FROM RFT_GET_GSB_CODE_INFO('BIMASAKTI', '{poParameter.CCOMPANY_ID}', '_GL_JOURNAL_STATUS', '', '{poParameter.CLANGUAGE_ID}') ORDER BY CCODE";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<StatusDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public List<CurrencyCodeDTO> GetCurrency(GLT00100ParameterDTO poParameter)
        {
            R_Exception loEx = new R_Exception();
            List<CurrencyCodeDTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = $"EXEC RSP_GS_GET_CURRENCY_LIST '{poParameter.CCOMPANY_ID}','{poParameter.CUSER_ID}'";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<CurrencyCodeDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public List<GetCenterDTO> GetCenterList(GLT00100ParameterDTO poParameter)
        {
            R_Exception loEx = new R_Exception();
            List<GetCenterDTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = $"EXEC RSP_GS_GET_CENTER_LIST '{poParameter.CCOMPANY_ID}','{poParameter.CUSER_ID}'";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GetCenterDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        public GSM_TRANSACTION_APPROVALDTO GetTransactionApproval(GLT00100ParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            GSM_TRANSACTION_APPROVALDTO loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                var lcQuery =
                    @"SELECT TOP 1 1 as CRESULT
                      FROM GSM_TRANSACTION_APPROVAL (NOLOCK)
                      WHERE CCOMPANY_ID = @CCOMPANY_ID
                      AND   CTRANS_CODE = @CTRANS_CODE
                      AND   CDEPT_CODE  = @CDEPT_CODE
                      AND   CUSER_ID    = @CUSER_ID";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CTRANS_CODE", DbType.String, 50, poParameter.CTRANS_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CDEPT_CODE", DbType.String, 50, poParameter.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<GSM_TRANSACTION_APPROVALDTO>(loReturnTemp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            return loResult;
        }
        #endregion
    }
}
