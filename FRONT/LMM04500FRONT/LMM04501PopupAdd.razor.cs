using GFF00900COMMON.DTOs;
using LMM04500COMMON.DTO_s;
using LMM04500MODEL;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Popup;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMM04500FRONT
{
    public partial class LMM04501PopupAdd : R_Page
    {
        private R_ConductorGrid _conGridPricing;
        private R_Grid<PricingBulkSaveDTO> _gridPricing;
        private LMM04500ViewModel _viewModel = new();

        [Inject] R_PopupService PopupService { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<PricingDTO>(poParameter);
                _viewModel._propertyId = loParam.CPROPERTY_ID;
                _viewModel._unitTypeCategoryId = loParam.CUNIT_TYPE_CATEGORY_ID;
                _viewModel._unitTypeCategoryName = loParam.CUNIT_TYPE_CATEGORY_NAME;
                _viewModel._validDate = loParam.CVALID_DATE;
                await _gridPricing.R_RefreshGrid(poParameter);//refresh grid param
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task PricingAdd_GetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _viewModel.GetPricingForSaveList();
                eventArgs.ListEntityResult = _viewModel._pricingSaveList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private void PricingAdd_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = R_FrontUtility.ConvertObjectToObject<PricingBulkSaveDTO>(eventArgs.Data);
        }

        private void PricingAdd_AfterAdd(R_AfterAddEventArgs eventArgs)
        {

        }


        #region Save Batch

        private async Task R_BeforeSaveBatchAsync(R_BeforeSaveBatchEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            GFF00900ParameterDTO loParam = null;
            R_PopupResult loResult = null;
            try
            {
                //data for approval trigger
                DateTime ldValidDate = DateTime.ParseExact(_viewModel._validDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                
                //Approval before saving
                if (ldValidDate < DateTime.Now)
                {
                    var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel
                    {
                        ACTIVATE_INACTIVE_ACTIVITY_CODE = "LMM04501"
                    };
                    await loValidateViewModel.RSP_ACTIVITY_VALIDITYMethodAsync();

                    if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" && loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
                    {
                        eventArgs.Cancel = false;
                    }
                    else
                    {
                        loParam = new GFF00900ParameterDTO()
                        {
                            Data = loValidateViewModel.loRspActivityValidityList,
                            IAPPROVAL_CODE = "LMM04501"
                        };
                        loResult = await PopupService.Show(typeof(GFF00900FRONT.GFF00900), loParam);
                        if (loResult.Success == false || (bool)loResult.Result == false)
                        {
                            await R_MessageBox.Show("", "Valid Date must be greater than Today",R_eMessageBoxButtonType.OK);
                            eventArgs.Cancel = true;
                        }
                    }
                }
                //~End approval
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                eventArgs.Cancel = true;
            loEx.ThrowExceptionIfErrors();
        }

        private void R_ServiceSaveBatchAsync(R_ServiceSaveBatchEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                _viewModel.SavePricing();
                //save action

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task R_AfterSaveBatchAsync(R_AfterSaveBatchEventArgs eventArgs)
        {
            await this.Close(true, null);
        }
        #endregion


    }
}
