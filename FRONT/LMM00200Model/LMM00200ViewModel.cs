using LMM00200Common;
using LMM00200Common.DTO_s;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LMM00200Model
{
    public class LMM00200ViewModel : R_ViewModel<LMM00200DTO>
    {
        private LMM00200Model _model = new LMM00200Model();

        public ObservableCollection<LMM00200StreamDTO> _UserParamList { get; set; } = new ObservableCollection<LMM00200StreamDTO>();
        public LMM00200DTO _UserParam { get; set; } = new LMM00200DTO();

        public string _CUserOperatorSign { get; set; } = "";
        public List<RadioModel> _Options { get; set; } = new List<RadioModel>
        {
            new RadioModel { Value = "=", Text = "(=)" },
            new RadioModel { Value= ">=", Text = "(>=)" },
        };
        public string _UserParamCode { get; set; }
        public bool _Active { get; set; }
        public string _Action { get; set; }

        public async Task GetUserParamList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _model.GetUserParamListAsync();
                _UserParamList = new ObservableCollection<LMM00200StreamDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetUserParamRecord(LMM00200DTO poParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CCODE, _UserParamCode);
                var loResult = await _model.R_ServiceGetRecordAsync(poParam);
                _UserParam = R_FrontUtility.ConvertObjectToObject<LMM00200DTO>(loResult);
                _CUserOperatorSign = _UserParam.CUSER_LEVEL_OPERATOR_SIGN;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveUserParam(LMM00200DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                _UserParam = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task ActiveInactiveProcessAsync(LMM00200DTO poParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                poParam.LACTIVE = _UserParam.LACTIVE;
                poParam.CACTION = _UserParam.CACTION;
                await _model.GetActiveParamAsync(poParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }

    public class RadioModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
