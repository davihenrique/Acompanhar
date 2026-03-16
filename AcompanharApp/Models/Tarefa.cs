namespace AcompanharApp.Models;

public class Tarefa
{
    public int Id { get; set; }
    public string Messagem { get; set; }
    public double Nota { get; set; }
    public int QuestionarioId { get; set; }
    public Questionario Questionario { get; set; }
}
