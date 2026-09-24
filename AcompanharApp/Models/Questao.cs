using System.Collections.Generic;

namespace AcompanharApp.Models;

public class Questao
{
    public int Id { get; set; }
    public string Enunciado { get; set; }
    public string Justificativa { get; set; }
    public List<Alternativa> Alternativas { get; set; }
    public int QuestionarioId { get; set; }
    public Questionario Questionario { get; set; }
}
