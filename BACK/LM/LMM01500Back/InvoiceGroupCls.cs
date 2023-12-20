using LMM01500Common.DTO;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;
using System.Data;

namespace LMM01500Back
{
    public class InvoiceGroupCls : R_BusinessObject<InvoiceGroupParamDTO>
    {
        protected override void R_Deleting(InvoiceGroupParamDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override InvoiceGroupParamDTO R_Display(InvoiceGroupParamDTO poEntity)
        {
            throw new NotImplementedException();
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

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<InvoiceGroupDTO>(loRtnTemp).ToList();
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
