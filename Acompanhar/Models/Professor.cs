using System.Collections.Generic;

namespace Acompanhar.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<Questionario> Questionarios { get; set; }
    }
}

