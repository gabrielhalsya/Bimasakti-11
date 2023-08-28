using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Windows.Input;
using LMM06500COMMON;
using System.Transactions;
using System.Data.SqlClient;

namespace LMM06500BACK
{
    public class LMM06502Cls : R_IBatchProcess
    {
        public List<LMM06502DetailDTO> GetAllStaffMove(LMM06502DetailDTO poEntity)
        {
            var loEx = new R_Exception();
            List<LMM06502DetailDTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = $"RSP_LM_GET_STAFF_BY_SUPERVISOR_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "@COLD_SUPERVISOR_ID", DbType.String, 50, poEntity.COLD_SUPERVISOR_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<LMM06502DetailDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            string lcPropertyId;
            string lcCmd;
            string lcQuery = "";
            var loDb = new R_Db();
            Dictionary<string, string> loMapping = new Dictionary<string, string>();
            List<LMM06501DTO> loResult = null;

            try
            {
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<LMM06502DTO>(poBatchProcessPar.BigObject);
                var loObject = R_Utility.R_ConvertCollectionToCollection<LMM06502DetailDTO, LMM06502DetailMoveDTO>(loTempObject.Detail);

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    DbConnection loConn = null;
                    DbCommand loCmd = null;

                    try
                    {
                        loConn = loDb.GetConnection();
                        loCmd = loDb.GetCommand();

                        lcQuery = "CREATE TABLE #STAFF_LIST (CSTAFF_ID	VARCHAR(20) ) ";

                        loDb.SqlExecNonQuery(lcQuery, loConn, false);

                        loDb.R_BulkInsert<LMM06502DetailMoveDTO>((SqlConnection)loConn, "#STAFF_LIST", loObject);

                        lcQuery = "RSP_LM_MOVE_STAFF";
                        loCmd.CommandText = lcQuery;
                        loCmd.CommandType = CommandType.StoredProcedure;

                        loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                        loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, loTempObject.Header.CPROPERTY_ID);
                        loDb.R_AddCommandParameter(loCmd, "@COLD_SUPERVISOR_ID", DbType.String, 50, loTempObject.Header.COLD_SUPERVISOR_ID);
                        loDb.R_AddCommandParameter(loCmd, "@CNEW_SUPERVISOR_ID", DbType.String, 50, loTempObject.Header.CNEW_SUPERVISOR_ID);
                        loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);

                        R_ExternalException.R_SP_Init_Exception(loConn);

                        try
                        {
                            loDb.SqlExecQuery(loConn, loCmd, false);
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
                    }
                    finally
                    {
                        if (loConn != null)
                        {
                            if (loConn.State != ConnectionState.Closed)
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
                    loException.ThrowExceptionIfErrors();

                    TransScope.Complete();
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

        public void SaveNewStaffMove(LMM06502DTO poEntity)
        {
            var loEx = new R_Exception();
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = null;
            var loCmd = loDb.GetCommand();

            try
            {

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loConn = loDb.GetConnection();

                    lcQuery = "DECLARE @CSTAFF_LIST AS RDT_TENANT_LIST ";

                    if (poEntity.Detail.Count > 0)
                    {
                        lcQuery += "INSERT INTO @CSTAFF_LIST " +
                            "(CTENANT_ID)" +
                            "VALUES ";
                        foreach (var loDetail in poEntity.Detail)
                        {
                            lcQuery += $"('{loDetail.CSTAFF_ID}'),";
                        }
                        lcQuery = lcQuery.Substring(0, lcQuery.Length - 1) + " ";

                    }

                    lcQuery += "EXECUTE RSP_LM_MOVE_STAFF " +
                        $"@CCOMPANY_ID = '{poEntity.Header.CCOMPANY_ID}' " +
                        $",@CPROPERTY_ID = '{poEntity.Header.CPROPERTY_ID}' " +
                        $",@COLD_SUPERVISOR_ID = '{poEntity.Header.COLD_SUPERVISOR_ID}' " +
                        $",@CNEW_SUPERVISOR_ID = '{poEntity.Header.CNEW_SUPERVISOR_ID}' " +
                        $",@CUSER_LOGIN_ID = '{poEntity.Header.CUSER_ID}' " +
                        ",@CSTAFF_LIST = @CSTAFF_LIST ";

                    R_ExternalException.R_SP_Init_Exception(loConn);

                    try
                    {
                        loDb.SqlExecQuery(lcQuery, loConn, false);
                    }
                    catch (Exception ex)
                    {
                        loEx.Add(ex);
                    }

                    loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));

                    TransScope.Complete();
                };
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

    }
}
