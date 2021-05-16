using System;
using System.Collections.Generic;


namespace Acompanhar.Models
{
    public class Questionario
    {
        public int Id { get; set; }
        public int ProfessorId { get; set; }
        public String Tema { get; set; }
        public List<Questao> Questoes { get; set; }
        public Questionario()
        {
            this.Questoes = new List<Questao>();
        }
    }
}