using LMM00200Common;
using LMM00200Common.DTO_s;
using LMM00200Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace LMM00200Front
{
    public partial class LMM00200 
    {
        private LMM00200ViewModel _viewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<LMM00200StreamDTO> _gridRef;
        private string _labelActiveInactive = "";
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModel.GetUserParamList();
                eventArgs.ListEntityResult = _viewModel._UserParamList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task Conductor_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<LMM00200DTO>(eventArgs.Data);
                _viewModel._UserParamCode = loParam.CCODE;
                await _viewModel.GetUserParamRecord(loParam);//getrecord
                _labelActiveInactive = loParam.LACTIVE ? "Inactive" : "Active"; //set label to button
                _viewModel._Active = !loParam.LACTIVE; //set active 
                _viewModel._Action = _labelActiveInactive.ToUpper(); //set action for context
                _viewModel._CUserOperatorSign = loParam.CUSER_LEVEL_OPERATOR_SIGN; //for user operator sign
                eventArgs.Result = _viewModel._UserParam;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public void Conductor_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (LMM00200DTO)eventArgs.Data;

                if (string.IsNullOrWhiteSpace(loData.CDESCRIPTION))
                    loEx.Add("", "Please fill Description.");

                if (string.IsNullOrWhiteSpace(loData.CVALUE))
                    loEx.Add("", "Please fill Value.");

                if (loData.IUSER_LEVEL < 0)
                    loEx.Add("", "User level start from 0.");
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = (LMM00200DTO)eventArgs.Data;
                loParam.CUSER_LEVEL_OPERATOR_SIGN = _viewModel._CUserOperatorSign;
                await _viewModel.SaveUserParam(loParam, (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = _viewModel._UserParam;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private void R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
        {
            eventArgs.GridData = R_FrontUtility.ConvertObjectToObject<LMM00200StreamDTO>(eventArgs.Data);
        }

        private bool _enableGridUserParam = true;
        private void R_SetOther(R_SetEventArgs eventArgs)
        {
            _enableGridUserParam = eventArgs.Enable;
        }

        #region Active/Inactive
        private async Task R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<LMM00200DTO>(_conductorRef.R_GetCurrentData());
                await _viewModel.ActiveInactiveProcessAsync(loParam);//do activeinactive
                await _viewModel.GetUserParamRecord(loParam);
                await _conductorRef.R_SetCurrentData(_viewModel._UserParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        #endregion
    }
}
