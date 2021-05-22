using Acompanhar.Enums;

namespace Acompanhar.ViewModels
{
    public class QuestionarioViewModel 
    {
 
        public string Question { get; set; }
        public string TextOption1St { get; set; }
        public string TextOption2Nd { get; set; }
        public string TextOption3Rd { get; set; }
        public string TextOption4Th { get; set; }
        public string TextOption5Th { get; set; }

        public string Label1St { get; set; }
        public string Label2Nd { get; set; }
        public string Label3Rd { get; set; }
        public string Label4Th { get; set; }
        public string Label5Th { get; set; }

        public bool Option1St { get; set; }
        public bool Option2Nd { get; set; }
        public bool Option3Rd { get; set; }
        public bool Option4Th { get; set; }
        public bool Option5Th { get; set; }
    }
}
