﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GLM00200COMMON
{
    public class GLM00200GSCenterDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CCENTER_CODE { get; set; }
        public string CCENTER_NAME { get; set; }
        public bool LACTIVE { get; set; }

        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
}
