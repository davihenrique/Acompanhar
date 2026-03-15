using AcompanharApp.Data;
using AcompanharApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace AcompanharApp.Repositories
{
    public class RealizaQuestionarioRepository : IRealizaQuestionarioRepository
    {
        private readonly AcompanharContext _context;
        public RealizaQuestionarioRepository(AcompanharContext context)
        {
            _context = context;
        }

        public bool IsQuestionario(int Code)
        {
            if (_context.Questionario.Any(q => q.Id == Code))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Questionario Questionario(int Code)
        {
            return (Questionario)_context.Questao.Where((System.Linq.Expressions.Expression<System.Func<Questao, bool>>)(q => q.Id == Code));
        }


        public List<Questao> ListQuestoes(int Code)
        {
            return _context.Questao.Where(q => q.QuestionarioId == Code).ToList();
        }
    }
}
