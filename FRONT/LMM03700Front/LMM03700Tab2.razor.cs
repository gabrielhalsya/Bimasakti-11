using LMM03700Common;
using LMM03700Common.DTO_s;
using LMM03700Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Tab;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace LMM03700Front
{
    public partial class LMM03700Tab2 : R_ITabPage
    {
        private LMM03710ViewModel _viewModelTenantClass = new();//viewModel TenantClass
        private LMM03700ViewModel _viewModelTenantClassGrp = new();//viewModel TenantClass
        private R_Conductor _conTenantClassGrpRef; //conductor grid TenantClassGrp tab 2
        private R_ConductorGrid _conTenantClassRef; //conductor grid TenantClass tab 2
        private R_ConductorGrid _conTenantRef; //conductor grid Tenant tab 2
        private R_Grid<TenantClassificationGroupDTO> _gridTenantClassGrpRef; //gridref TenantClassGrp tab 2
        private R_Grid<TenantClassificationDTO> _gridTenantClassRef; //gridref TenantClass tab 2
        private R_Grid<TenantDTO> _gridTenantRef; //gridref Tenant tab 2
        private R_Popup R_PopupCheck;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                _viewModelTenantClass._propertyId = (string)poParameter;
                await _gridTenantClassGrpRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task RefreshTabPageAsync(object poParam)
        {
            _viewModelTenantClass._propertyId = (string)poParam;
            await _gridTenantClassGrpRef.R_RefreshGrid(null);
        }

        #region TenantClassGrp
        private async Task TenantClassGrp_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _viewModelTenantClass.GetTenantClassGroupList();
                eventArgs.ListEntityResult = _viewModelTenantClass.TenantClassGrpList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
        private async Task TenantClassGrp_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationGroupDTO>(eventArgs.Data);
                await _viewModelTenantClass.GetTenantClassGroupRecord(loParam);
                eventArgs.Result = _viewModelTenantClass.TenantClassiGrp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task TenantClassGrp_ServiceDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                _viewModelTenantClass._tenantClassificationGroupId = loParam.CTENANT_CLASSIFICATION_GROUP_ID;
                await _gridTenantClassRef.R_RefreshGrid(null);
                //await _gridTCRef.AutoFitAllColumnsAsync();
                //_viewTCModel.AssignedTenantList = null;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                throw;
            }
        }
        #endregion

        #region TenantClass
        private async Task TenantClass_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _viewModelTenantClass.GetTenantClassList();
                eventArgs.ListEntityResult = _viewModelTenantClass.TenantClassList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
        private async Task TenantClass_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                await _viewModelTenantClass.GetTenantClassRecord(loParam);
                eventArgs.Result = _viewModelTenantClass.TenantClass;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task TenantClass_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                await _viewModelTenantClass.DeleteTenantClass(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task TenantClass_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                await _viewModelTenantClass.SaveTenantClass(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _viewModelTenantClass.TenantClass;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private async Task TenantClass_ServiceDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loEntity = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                _viewModelTenantClass._tenantClassificationId = loEntity.CTENANT_CLASSIFICATION_ID;
                await _gridTenantRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Tab2-TenantList
        private async Task Tenant_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //_viewTCModel._tenantClassificationId = (string)eventArgs.Parameter;
                await _viewModelTenantClass.GetAssignedTenantList();
                eventArgs.ListEntityResult = _viewModelTenantClass.AssignedTenantList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        private void Tenant_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.Result = R_FrontUtility.ConvertObjectToObject<TenantDTO>(_gridTenantRef.GetCurrentData());
                if (eventArgs.Result == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        #endregion

        #region Tab2-Assign Tenant
        private void R_Before_Open_Popup_AssignTenant(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loParam = new TenantGridPopupDTO()
            {
                CPROPERTY_ID = _viewModelTenantClass._propertyId,
                CTENANT_CLASSIFICATION_GROUP_ID = _viewModelTenantClass._tenantClassificationGroupId,
                CTENANT_CLASSIFICATION_ID = _viewModelTenantClass._tenantClassificationId
            };
            eventArgs.Parameter = loParam;
            eventArgs.TargetPageType = typeof(PopupAssignTenant);
        }
        private async Task R_After_Open_Popup_AssignTenant(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                if (eventArgs.Result == null)
                {
                    return;
                }
                var loResult = (List<SelectedTenantGridPopupDTO>)eventArgs.Result;
                var loSelectedResult = loResult.Where(obj => (bool)obj.GetType().GetProperty("LSELECTED").GetValue(obj)).ToList();

                var loAssignTenantParam = R_FrontUtility.ConvertCollectionToCollection<TenantGridPopupDTO>(loSelectedResult);
                if (loAssignTenantParam.Count > 0)
                {
                    await _viewModelTenantClass.AssignTenantCategory(loAssignTenantParam.ToList());
                    await _gridTenantRef.R_RefreshGrid(null);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Tab2-Move Tenant
        private void R_Before_Open_Popup_MoveTenant(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loParam = new TenantGridPopupDTO()
            {
                CPROPERTY_ID = _viewModelTenantClass._propertyId,
                CTENANT_CLASSIFICATION_GROUP_ID = _viewModelTenantClass._tenantClassificationGroupId,
                CTENANT_CLASSIFICATION_ID = _viewModelTenantClass._tenantClassificationId,
            };
            eventArgs.Parameter = loParam;
            eventArgs.TargetPageType = typeof(PopupMoveTenant);
        }
        private async Task R_After_Open_Popup_MoveTenant(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                if (eventArgs.Result == null)
                {
                    return;
                }
                _viewModelTenantClass._tenantClassificationId = (string)eventArgs.Result;
                await _gridTenantClassRef.R_RefreshGrid(null);
                //await _gridTCRef.AutoFitAllColumnsAsync();
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
