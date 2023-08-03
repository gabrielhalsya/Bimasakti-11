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
        private LMM03700ViewModel _vmTenantClassGrp = new(); //ViewModel TenantClassGrp
        private LMM03710ViewModel _vmTenantClass = new();//viewModel TenantClass
        private R_Conductor _conTenantClassGrp; //conductor grid TenantClassGrp tab 2
        private R_ConductorGrid _conTenantClass; //conductor grid TenantClass tab 2
        private R_ConductorGrid _conTenant; //conductor grid Tenant tab 2
        private R_Grid<TenantClassificationGroupDTO> _gridTenantClassGrp; //gridref TenantClassGrp tab 2
        private R_Grid<TenantClassificationDTO> _gridTenantClass; //gridref TenantClass tab 2
        private R_Grid<TenantDTO> _gridTenant; //gridref Tenant tab 2

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                _vmTenantClassGrp._propertyId = (string)poParameter;
                _vmTenantClass._propertyId = (string)poParameter;
                await _gridTenantClassGrp.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task RefreshTabPageAsync(object poParam)
        {
            _vmTenantClassGrp._propertyId = (string)poParam;
            _vmTenantClass._propertyId = (string)poParam;
            await _gridTenantClassGrp.R_RefreshGrid(null);
        }

        #region Tab2-TenantClassificationGrp
        private async Task TenantClassGroup_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _vmTenantClass.GetTenantClassGroupList();
                eventArgs.ListEntityResult = _vmTenantClass.TenantClassGrpList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);

        }
        private async Task TenantClassGroup_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationGroupDTO>(eventArgs.Data);
                await _vmTenantClass.GetTenantClassGroupRecord(loParam);
                eventArgs.Result = _vmTenantClass.TenantClassiGrp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        private async Task TenantClassGroup_ServiceDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<TenantClassificationDTO>(eventArgs.Data);
                _vmTenantClass._tenantClassificationGroupId = loParam.CTENANT_CLASSIFICATION_GROUP_ID;
                await _gridTenantClass.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                throw;
            }
        }
        #endregion

        #region Tab2-TenantClassification
        private async Task TenantClass_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _vmTenantClass.GetTenantClassList();
                eventArgs.ListEntityResult = _vmTenantClass.TenantClassList;
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
                await _vmTenantClass.GetTenantClassRecord(loParam);
                eventArgs.Result = _vmTenantClass.TenantClass;
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
                await _vmTenantClass.DeleteTenantClass(loParam);
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
                await _vmTenantClass.SaveTenantClass(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _vmTenantClass.TenantClass;
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
                _vmTenantClass._tenantClassificationId = loEntity.CTENANT_CLASSIFICATION_ID;
                await _gridTenant.R_RefreshGrid(null);
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
                await _vmTenantClass.GetAssignedTenantList();
                eventArgs.ListEntityResult = _vmTenantClass.AssignedTenantList;
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
                eventArgs.Result = R_FrontUtility.ConvertObjectToObject<TenantDTO>(_gridTenant.GetCurrentData());
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
        private void R_Before_Open_PopupAssignTenant(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loParam = new TenantGridPopupDTO()
            {
                CPROPERTY_ID = _vmTenantClass._propertyId,
                CTENANT_CLASSIFICATION_GROUP_ID = _vmTenantClass._tenantClassificationGroupId,
                CTENANT_CLASSIFICATION_ID = _vmTenantClass._tenantClassificationId
            };
            eventArgs.Parameter = loParam;
            eventArgs.TargetPageType = typeof(PopupAssignTenant);
        }
        private async Task R_After_Open_PopupAssignTenant(R_AfterOpenPopupEventArgs eventArgs)
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
                    await _vmTenantClass.AssignTenantCategory(loAssignTenantParam);
                    await _gridTenant.R_RefreshGrid(null);
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
                CPROPERTY_ID = _vmTenantClass._propertyId,
                CTENANT_CLASSIFICATION_GROUP_ID = _vmTenantClass._tenantClassificationGroupId,
                CTENANT_CLASSIFICATION_ID = _vmTenantClass._tenantClassificationId,
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
                _vmTenantClass._tenantClassificationId = (string)eventArgs.Result;
                await _gridTenantClass.R_RefreshGrid(null);
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
