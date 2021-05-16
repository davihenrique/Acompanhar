using Acompanhar.Enums;

namespace Acompanhar.ViewModels
{
    public class QuestionarioViewModel 
    {
 
        public string Enuciado { get; set; }
        public string AlternativaA { get; set; }
        public string AlternativaB { get; set; }
        public string AlternativaC { get; set; }
        public string AlternativaD { get; set; }
        public string AlternativaE { get; set; }

        public string RotuloA { get; set; }
        public string RotuloB { get; set; }
        public string RotuloC { get; set; }
        public string RotuloD { get; set; }
        public string RotuloE { get; set; }

        public bool Option1St { get; set; }
        public bool Option2Nd { get; set; }
        public bool Option3Rd { get; set; }
        public bool Option4Th { get; set; }
        public bool Option5Th { get; set; }




        /*
        public QuestionarioViewModel(Questionario questionario, List<Questao> questoes)
        {
            Questionario = questionario;
            Questoes = questoes;
        }*/
    }
}
