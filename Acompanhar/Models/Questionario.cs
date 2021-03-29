using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Models
{
    public class Questionario
    {
        public int Id { get; set; }
        public int IdProfessor { get; set; }
        public String Tema { get; set; }

        public Questao Questao1 { get; set; }
        public Questao Questao2 { get; set; }
        public Questao Questao3 { get; set; }
        public Questao Questao4 { get; set; }
        public Questao Questao5 { get; set; }
        public Questao Questao6 { get; set; }
        public Questao Questao7 { get; set; }
        public Questao Questao8 { get; set; }
        public Questao Questao9 { get; set; }
        public Questao Questao10 { get; set; }

        public virtual Questao Questao { get; set; }





    }
}
