using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Models
{
    public class Questao
    {
        public int Id { get; set; }
        public String Enunciado { get; set; }
        public List<Alternativa> Alternativas { get; set; }
        public String Justificativa { get; set; }
        public String Resposta { get; set; }

        public Questao()
        {
            this.Alternativas = new List<Alternativa>();
        }
    }
}
