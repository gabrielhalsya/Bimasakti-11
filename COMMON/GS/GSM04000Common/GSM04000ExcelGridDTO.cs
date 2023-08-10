using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM04000Common
{
    public class GSM04000ExcelGridDTO : GSM04000DTO

    { 
        public bool LSELECTED { get; set; }
        public bool LOVERWRITE { get; set; }
        public bool LEXISTS { get; set; }
        public string CERROR_MESSAGE { get; set; }
        public string CNON_ACTIVE_DATE{ get; set; } //YYYYMMDD
    }

    public class GSM04000ListExcelGridDTO : R_APIResultBaseDTO
    {
        public List<GSM04000ExcelGridDTO> Data { get; set; }   
    }
}
