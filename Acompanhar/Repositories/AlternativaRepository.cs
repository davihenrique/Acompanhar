using Acompanhar.Data;
using Acompanhar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acompanhar.Repositories
{
    public class AlternativaRepository : IAlternativaRepository
    {
        private readonly AcompanharContext _context;
        public AlternativaRepository(AcompanharContext context)
        {
            _context = context;
        }
        public int GetAlternativaCont(int IdQuestao)
        {
            return _context.Alternativa.Count(a => a.QuestaoId == IdQuestao);
        }
        public IEnumerable<Alternativa> GetAlternativasOrder(int IdQuestao)
        {
            return _context.Alternativa.Where(a => a.QuestaoId == IdQuestao).OrderBy(a => a.Id).ToList();
        }
    }
}
