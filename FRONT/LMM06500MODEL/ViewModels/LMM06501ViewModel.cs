using LMM06500COMMON;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMM06500MODEL
{
    public class LMM06501ViewModel : R_IProcessProgressStatus
    {
        private LMM06501Model _LMM06501Model = new LMM06501Model();
        public bool Var_Exists { get; set; }
        public bool Var_Overwrite { get; set; }
        public Action StateChangeAction { get; set; }

        public string PropertyValue = "";
        public string PropertyName = "";
        public string SourceFileName = "";
        public string Message = "";
        public int Percentage = 0;
        public bool OverwriteData = false;
        public byte[] fileByte = null;
        public bool VisibleError = false;
        public bool BtnSave = false;

        public ObservableCollection<LMM06501DTO> StaffUploadGrid { get; set; } = new ObservableCollection<LMM06501DTO>();
        public ObservableCollection<LMM06500DTO> StaffGrid { get; set; } = new ObservableCollection<LMM06500DTO>();

        public ObservableCollection<LMM06501ErrorValidateDTO> StaffValidateUploadError { get; set; } = new ObservableCollection<LMM06501ErrorValidateDTO>();
        public ObservableCollection<LMM06501DTO> StaffValidateUpload { get; set; } = new ObservableCollection<LMM06501DTO>();

        public async Task GetStaffList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM06501Model.GetStaffUploadListAsync(PropertyValue);

                StaffGrid = new ObservableCollection<LMM06500DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task AttachFile(List<LMM06501DTO> poBigObject, string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loUploadPar;
            R_ProcessAndUploadClient loCls;
            List<LMM06501DTO> Bigobject;
            List<R_KeyValue> loUserParameneters;
            R_IProcessProgressStatus loProgressStatus;

            try
            {
                loUserParameneters = new List<R_KeyValue>();
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = PropertyValue });

                //preapare Batch Parameter
                loUploadPar = new R_BatchParameter();
                loUploadPar.COMPANY_ID = pcCompanyId;
                loUploadPar.USER_ID = pcUserId;
                loUploadPar.UserParameters = loUserParameneters;
                loUploadPar.ClassName = "LMM06500BACK.LMM06500UploadStaffCls";
                loUploadPar.BigObject = poBigObject;

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    poProcessProgressStatus: this,
                    pcModuleName: "LM",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrlLM");

                var loKeyGuid = await loCls.R_BatchProcess<List<LMM06501DTO>>(loUploadPar, 20);

                StaffValidateUpload = new ObservableCollection<LMM06501DTO>(poBigObject);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

        }

        public async Task ValidateDataList(List<LMM06501DTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await GetStaffList();

                var loMasterData = StaffGrid.Where(x => x.CPROPERTY_ID == PropertyValue).Select(x => x.CSTAFF_ID).ToList();

                var loData = poEntity.Select(item =>
                {
                    item.Var_Exists = loMasterData.Contains(item.StaffId);
                    return item;
                }).ToList();

                await ConvertGrid(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }

        public async Task ConvertGrid(List<LMM06501DTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loTempParam = poEntity;
                var loData = loTempParam.Select(loTemp => new LMM06501ErrorValidateDTO
                {
                    StaffId = loTemp.StaffId,
                    StaffName = loTemp.StaffName,
                    Active = loTemp.Active,
                    Department = loTemp.Department,
                    Position = loTemp.Position,
                    JoinDate = loTemp.JoinDate,
                    Supervisor = loTemp.Supervisor,
                    EmailAddress = loTemp.EmailAddress,
                    MobileNo1 = loTemp.MobileNo1,
                    MobileNo2 = loTemp.MobileNo2,
                    Gender = loTemp.Gender,
                    Address = loTemp.Address,
                    InActiveDate = DateTime.ParseExact(loTemp.InActiveDate, "yyyyMMdd", CultureInfo.InvariantCulture),
                    InactiveNote = loTemp.InactiveNote,
                    Var_Exists = loTemp.Var_Exists,
                    Var_Overwrite = loTemp.Var_Overwrite
                }).ToList();

                StaffValidateUploadError = new ObservableCollection<LMM06501ErrorValidateDTO>(loData);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }

        public async Task SaveBulkFile(string pcCompanyId, string pcUserId)
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<LMM06501ErrorValidateDTO> Bigobject;
            List<R_KeyValue> loUserParameneters;

            try
            {
                // set Param
                loUserParameneters = new List<R_KeyValue>();
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CPROPERTY_ID, Value = PropertyValue });
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.COVERWRITE, Value = OverwriteData });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "LM",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrlLM",
                    poProcessProgressStatus: this);

                //Set Data
                if (StaffValidateUploadError.Count == 0)
                    return;

                Bigobject = StaffValidateUploadError.ToList<LMM06501ErrorValidateDTO>();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = pcCompanyId;
                loBatchPar.USER_ID = pcUserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "LMM06500BACK.LMM06501Cls";
                loBatchPar.BigObject = Bigobject;
                var lcGuid = await loCls.R_BatchProcess<List<LMM06501ErrorValidateDTO>>(loBatchPar, 20);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Status
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);

                VisibleError = false;
                BtnSave = true;

                await ValidateDataList(StaffValidateUpload.ToList());
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                if (StaffUploadGrid.Count > 0)
                {
                    Message = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
                }
                else
                {
                    Message = "Process Complete but fail !!! Get Error Validate Data";
                }

                BtnSave = false;
                VisibleError = true;

                await GetError(pcKeyGuid);
            }

            StateChangeAction();
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

            foreach (R_APICommonDTO.R_Error item in ex.ErrorList)
            {
                Message = string.Format($"{item.ErrDescp}");
            }

            StateChangeAction();

            await Task.CompletedTask;
        }

        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            await Task.CompletedTask;
        }

        private async Task GetError(string pcKeyGuid)
        {
            R_APIException loException;

            try
            {
                var loError = await _LMM06501Model.GetErrorProcessAsync(pcKeyGuid);

                StaffValidateUploadError = new ObservableCollection<LMM06501ErrorValidateDTO>(loError);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
        #endregion
    }

}
