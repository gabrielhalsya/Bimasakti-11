using LMM00200Common;
using LMM00200Common.DTO_s;
using LMM00200Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace LMM00200Front
{
    public partial class LMM00200
    {
        private LMM00200ViewModel _viewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<LMM00200GridDTO> _gridRef;
        private string _labelActiveInactive = "";
        private R_NumericTextBox<int> _numTextBoxUserLevel; //ref for Value textbox

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

            loEx.ThrowExceptionIfErrors();
        }
        private async Task UserParam_ServiceGetListRecordAsync(R_ServiceGetListRecordEventArgs eventArgs)
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
        private async Task UserParam_ServiceGetRecordAsync(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<LMM00200DTO>(eventArgs.Data);
                await _viewModel.GetUserParamRecord(loParam);//getrecord
                _labelActiveInactive = _viewModel._UserParam.LACTIVE ? "Inactive" : "Active"; //set label to button
                _viewModel._Active = !_viewModel._UserParam.LACTIVE; //set active 
                _viewModel._Action = _labelActiveInactive.ToUpper(); //set action for context
                _viewModel._CUserOperatorSign = _viewModel._UserParam.CUSER_LEVEL_OPERATOR_SIGN; //for user operator sign
                eventArgs.Result = _viewModel._UserParam;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void UserParam_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (LMM00200DTO)eventArgs.Data;
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
        private void UserParam_Saving(R_SavingEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (LMM00200DTO)eventArgs.Data;
                if (string.IsNullOrEmpty(loData.CVALUE)) //make sure cvalue not null while sending to back
                {
                    loData.CVALUE = "";
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task UserParam_ServiceSaveAsync(R_ServiceSaveEventArgs eventArgs)
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
        private void UserParam_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.GridData = R_FrontUtility.ConvertObjectToObject<LMM00200DTO>(eventArgs.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task UserParam_SetEditAsync(R_SetEventArgs eventArgs)
        {
            //await _numTextBoxUserLevel.FocusAsync(); //make focus to textboxvalue
            //eventArgs.Cancel = true;
        }
        private async Task BtnActiveInactive()
        {
            R_Exception loEx = new R_Exception();

            try
            {
                var loData = _conductorRef.R_GetCurrentData() as LMM00200DTO; //get current data
                var loParam = R_FrontUtility.ConvertObjectToObject<ActiveInactiveParam>(loData);
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
        private bool _gridEnabled = true;
        private void UserParam_SetOther(R_SetEventArgs eventArgs)
        {
            _gridEnabled = eventArgs.Enable;
        }
        private async Task UserParam_DisplayAsync(R_DisplayEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                if (eventArgs.ConductorMode== R_eConductorMode.Edit)
                {
                    await _numTextBoxUserLevel.FocusAsync(); //make focus to textboxvalue
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }
}
