using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Models
{
    public class Questao
    {
        public int Id { get; set; }
        public int IdQuestionario { get; set; }

        public String Enuciado { get; set; }
        public String AlternativaA { get; set; }
        public String AlternativaB { get; set; }
        public String AlternativaC { get; set; }
        public String AlternativaD { get; set; }
        public String AlternativaE { get; set; }
        public String Justificativa { get; set; }
        public String Resposta { get; set; }
    }
}
