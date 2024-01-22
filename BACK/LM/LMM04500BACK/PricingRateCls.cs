using LMM04500COMMON;
using R_BackEnd;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500BACK
{
    public class PricingRateCls : R_BusinessObject<PricingRateDTO>
    {
        protected override void R_Deleting(PricingRateDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override PricingRateDTO R_Display(PricingRateDTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(PricingRateDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }
    }
}
