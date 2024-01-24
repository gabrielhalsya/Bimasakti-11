﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LMM04500COMMON.DTO_s
{
    public class PricingDTO
    {
        public string CCOMPANY_ID {get;set;}
        public string CPROPERTY_ID {get;set;}
        public string CPRICE_TYPE {get;set;}
        public string CUNIT_TYPE_CATEGORY_ID {get;set;}
        public string CUNIT_TYPE_CATEGORY_NAME {get;set;}
        public string CVALID_INTERNAL_ID {get;set;}
        public string CVALID_DATE {get;set;}
        public DateTime DVALID_DATE {get;set; }
        public string CCHARGES_TYPE {get;set;}
        public string CCHARGES_TYPE_DESCR {get;set;}
        public string CCHARGES_ID {get;set;}
        public string CCHARGES_NAME {get;set;}
        public string CPRICE_MODE {get;set;}
        public string CPRICE_MODE_DESCR {get;set;}
        public decimal NNORMAL_PRICE {get;set;}
        public decimal NBOTTOM_PRICE {get;set;}
        public bool LOVERWRITE {get;set;}
        public bool LACTIVE {get;set;}
        public string CACTIVE_BY {get;set;}
        public DateTime DACTIVE_DATE {get;set;}
        public string CINACTIVE_BY {get;set;}
        public DateTime DINACTIVE_DATE {get;set;}
        public string CCREATE_BY {get;set;}
        public DateTime DCREATE_DATE {get;set;}
        public string CUPDATE_BY {get;set;}
        public DateTime DUPDATE_DATE { get; set; }
    }
}
