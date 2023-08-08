using GLM00200Common;
using GLM00200Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLM00200Front
{
    public partial class RapidApprovalRecurringJrn : R_Page
    {
        private GLM00200ViewModel _journalVM = new GLM00200ViewModel();
        private R_Grid<JournalGridDTO> _gridJournal;
        private R_ConductorGrid _conJournal;

    }
}
