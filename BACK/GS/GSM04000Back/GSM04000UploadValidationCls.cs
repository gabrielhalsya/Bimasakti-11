using GSM04000Common;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GSM04000Back
{
    public class GSM04000UploadValidationCls : R_IBatchProcess
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

                    lcQuery = "RSP_GS_VALIDATE_UPLOAD_DEPARTMENT";
                    loCommand.CommandText = lcQuery;
                    loCommand.CommandType = CommandType.StoredProcedure;

                    loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);

                    loDb.R_AddCommandParameter(loCommand, "@CKEY_GUID", DbType.String, 20, poBatchProcessPar.Key.KEY_GUID);

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
        

    }
}
