﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LMM04500COMMON.DTO_s
{
    public class PricingBulkSaveDTO
    {
        public int ISEQ { get; set; }
        public string CVALID_INTERNAL_ID { get; set; }
        public string CCHARGES_TYPE { get; set; }
        public string CCHARGES_ID { get; set; }
        public string CPRICE_MODE { get; set; }
        public decimal NNORMAL_PRICE { get; set; }
        public decimal NBOTTOM_PRICE { get; set; }
        public bool LOVERWRITE { get; set; }
    }
}
