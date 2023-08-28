using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using LMM06500COMMON;
using System.Data.SqlClient;
using System.Data.Common;
using System.Transactions;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System;
using System.Globalization;
using System.Text.Json;

namespace LMM06500BACK
{
    public class LMM06501Cls : R_IBatchProcess
    {
        public List<LMM06500DTO> GetAllStaffUpload(LMM06500DTO poEntity)
        {
            var loEx = new R_Exception();
            List<LMM06500DTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                string lcQuery = $"SELECT * FROM LMM_STAFF (NOLOCK) WHERE CCOMPANY_ID = @CCOMPANY_ID AND CPROPERTY_ID = @CPROPERTY_ID";
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<LMM06500DTO>(loDataTable).ToList();

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
            string lcQuery = "";
            var loDb = new R_Db();
            DbCommand loCmd = null;
            DbConnection loConn = null;
            try
            {
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<LMM06501ErrorValidateDTO>>(poBatchProcessPar.BigObject);
                var loObject = loTempObject.Select(loTemp => new LMM06501RequestDTO
                {
                    StaffId = loTemp.StaffId,
                    StaffName = loTemp.StaffName,
                    Active = loTemp.Active,
                    Department = loTemp.Department,
                    Position = loTemp.Position,
                    JoinDate = loTemp.JoinDate,
                    Supervisor = loTemp.Supervisor,
                    EmailAddress = loTemp.EmailAddress,
                    MobileNo1 = loTemp.MobileNo1,
                    MobileNo2 = loTemp.MobileNo2,
                    Gender = loTemp.Gender,
                    Address = loTemp.Address,
                    InActiveDate = loTemp.InActiveDate.ToString("yyyyMMdd"),
                    InactiveNote = loTemp.InactiveNote
                }).ToList();

                var loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CPROPERTY_ID)).FirstOrDefault().Value;
                var lcPropertyId = ((System.Text.Json.JsonElement)loVar).GetString();

                var loTempVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.COVERWRITE)).FirstOrDefault().Value;
                var lbOverwrite = ((System.Text.Json.JsonElement)loTempVar).GetBoolean();

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loConn = loDb.GetConnection();
                    loCmd = loDb.GetCommand();

                    lcQuery += "CREATE TABLE #STAFF (	NO INT " +
                            ", StaffId VARCHAR(20) " +
                            ", StaffName NVARCHAR(100) " +
                            ", Active BIT " +
                            ", Department VARCHAR(20) " +
                            ", Position VARCHAR(2) " +
                            ", JoinDate VARCHAR(8) " +
                            ", Supervisor VARCHAR(20) " +
                            ", EmailAddress NVARCHAR(100) " +
                            ", MobileNo1 VARCHAR(30) " +
                            ", MobileNo2 VARCHAR(30) " +
                            ", Gender VARCHAR(2) " +
                            ", Address NVARCHAR(255) " +
                            ", InActiveDate VARCHAR(8) " +
                            ", InactiveNote  NVARCHAR(255) ) ";

                    loDb.SqlExecNonQuery(lcQuery, loConn, false);

                    loDb.R_BulkInsert<LMM06501RequestDTO>((SqlConnection)loConn, "#STAFF", loObject);

                    lcQuery = "EXECUTE RSP_LM_UPLOAD_STAFF @CCOMPANY_ID, @CPROPERTY_ID, @CUSER_ID, @CKEY_GUID, @LOVERWRITE";
                    loCmd.CommandText = lcQuery;

                    loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, lcPropertyId);
                    loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);
                    loDb.R_AddCommandParameter(loCmd, "@CKEY_GUID", DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                    loDb.R_AddCommandParameter(loCmd, "@LOVERWRITE", DbType.Boolean, 20, lbOverwrite);

                    loDb.SqlExecNonQuery(loConn, loCmd, false);

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
                    loConn = null;
                }
                if (loCmd != null)
                {
                    loCmd.Dispose();
                    loCmd = null;
                }
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}
