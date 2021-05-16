namespace Acompanhar.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public int QuestionarioId { get; set; }
        public string Messagem { get; set; }
        public double Nota { get; set; }

    }
}
