using R_BlazorFrontEnd.Controls;

namespace GLM00200Front
{
    public partial class ActualJournalList : R_Page
    {
        public string Title { get; set; }
        protected override Task R_Init_From_Master(object poParameter)
        {
            Title = (string)poParameter;

            return Task.CompletedTask;
        }
    }
}
