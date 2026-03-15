using System.Collections.Generic;

namespace AcompanharApp.ViewModels
{
    public class QuestionarioViewModel 
    {
        public string Question { get; set; }

        public List<AlternativeViewModel> Alternatives { get; set; }
    }
}
