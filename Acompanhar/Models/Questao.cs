using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Models
{
    public class Questao
    {
        public int QuestaoId { get; set; }

        public string Enunciado { get; set; }
        public string AlternativaA { get; set; }
        public string AlternativaB { get; set; }
        public string AlternativaC { get; set; }
        public string AlternativaD { get; set; }
        public string AlternativaE { get; set; }
        public string Justificativa { get; set; }
        public string Resposta { get; set; }

        public virtual Questionario Questionarios { get; set; }
    }
}
