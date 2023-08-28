﻿using LMM06500BACK;
using LMM06500COMMON;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMM06500SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LMM06500UniversalController : ControllerBase, ILMM06500Universal
    {
        [HttpPost]
        public IAsyncEnumerable<LMM06500UniversalDTO> GetPositionList(LMM06500UniversalDTO poParam)
        {
            var loEx = new R_Exception();
            IAsyncEnumerable<LMM06500UniversalDTO> loRtn = null;

            try
            {
                var loCls = new LMM06500UniversalCls();

                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                var loResult = loCls.GetAllPosition(poParam);

                loRtn = GetStream<LMM06500UniversalDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<LMM06500UniversalDTO> GetGenderList(LMM06500UniversalDTO poParam)
        {
            var loEx = new R_Exception();
            IAsyncEnumerable<LMM06500UniversalDTO> loRtn = null;

            try
            {
                var loCls = new LMM06500UniversalCls();

                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                var loResult = loCls.GetAllGender(poParam);

                loRtn = GetStream<LMM06500UniversalDTO>(loResult);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        private async IAsyncEnumerable<T> GetStream<T>(List<T> poList)
        {
            foreach (var item in poList)
            {
                yield return item;
            }
        }
    }
}
