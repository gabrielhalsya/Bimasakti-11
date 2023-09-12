using GSM04000Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GSM04000Back
{
    public class GSM00400UploadCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            string lcQuery = "";
            R_Db loDb = null;
            DbConnection loConn = null;
            var loCommand = loDb.GetCommand();

            try
            {
                loDb = new R_Db();
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<GSM04000ExcelToUploadDTO>>(poBatchProcessPar.BigObject);
                var loObject = loTempObject.Select(loTemp => new GSM04000ExcelToUploadDTO
                {
                    DepartmentCode = loTemp.DepartmentCode,
                    DepartmentName = loTemp.DepartmentName,
                    CenterCode = loTemp.CenterCode,
                    ManagerName = loTemp.ManagerName,
                    Everyone = loTemp.Everyone,
                    Active = loTemp.Active,
                    NonActiveDate = loTemp.NonActiveDate,
                }).ToList();

                //get parameter


                var loTempVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.LOVERWRITE)).FirstOrDefault().Value;
                var lbOverwrite = ((System.Text.Json.JsonElement)loTempVar).GetBoolean();

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loConn = loDb.GetConnection();

                    lcQuery = $"CREATE TABLE #DEPARTMENT " +
                              $"(No INT, " +
                              $"DepartmentCode VARCHAR(8), " +
                              $"DepartmentName VARCHAR(80), " +
                              $"CenterCode VARCHAR(8)," +
                              $"ManagerName VARCHAR(8)," +
                              $"Everyone BIT," +
                              $"Active BIT," +
                              $"NonActiveDate VARCHAR(8))";

                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    loDb.R_BulkInsert<GSM04000ExcelToUploadDTO>((SqlConnection)loConn, "#DEPARTMENT", loObject);

                    lcQuery = "RSP_GS_UPLOAD_DEPARTMENT";
                    loCommand.CommandText = lcQuery;
                    loCommand.CommandType = CommandType.StoredProcedure;

                    loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);

                    loDb.R_AddCommandParameter(loCommand, "@CKEY_GUID", DbType.String, 20, poBatchProcessPar.Key.KEY_GUID);
                    loDb.R_AddCommandParameter(loCommand, "@LOVERWRITE", DbType.Boolean, 20, lbOverwrite);

                    loDb.SqlExecNonQuery(loConn, loCommand, false);
                    TransScope.Complete();
                }
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
                }
            }

            loException.ThrowExceptionIfErrors();
        }

        public List<GSM04000ExcelGridDTO> GetErrorProcess(string pcCompanyId, string pcUserId, string pcKeyGuid)
        {
            var loEx = new R_Exception();
            var lcQuery = "";
            var loDb = new R_Db();
            List<GSM04000ExcelGridDTO> loResult = null;
            DbConnection loConn = null;

            try
            {


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
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }
            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public List<GSM04000DTO> GetDeptDataToCompare(string piCompanyId)
        {
            R_Exception loException = new R_Exception();
            List<GSM04000DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                string lcQuery = $"SELECT * FROM GSM_DEPARTMENT (NOLCOK) WHERE CCOMPANY_ID = @CCOMPANY_ID";
                loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, piCompanyId);
                var loResult = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM04000DTO>(loResult).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            return loRtn;
        }

    }
}
