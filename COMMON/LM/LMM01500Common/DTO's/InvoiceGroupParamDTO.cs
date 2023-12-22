using System;
using System.Collections.Generic;
using System.Text;

namespace LMM01500Common
{
    public class InvoiceGroupParamDTO : InvoiceGroupDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CACTION { get; set; }
        public string CUSER_ID { get; set; }
    }
}
