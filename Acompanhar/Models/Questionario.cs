using System.Collections.Generic;


namespace Acompanhar.Models
{
    public class Questionario
    {
        public int Id { get; set; }
        public string Tema { get; set; }
        public List<Questao> Questoes { get; set; }
        public List<Tarefa> Tarefas { get; set; }
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }
    }
}