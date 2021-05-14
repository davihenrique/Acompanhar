namespace Acompanhar.Models
{
    public class Alternativa
    {
        public int Id { get; set; }
        public int QuestaoId { get; set; }
        public string Rotulo { get; set; }
        public string Afirmacao { get; set; }
        public bool Veracidade { get; set; }
    }
}