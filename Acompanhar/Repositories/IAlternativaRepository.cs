using Acompanhar.Models;
using System.Collections.Generic;

namespace Acompanhar.Repositories
{
    public interface IAlternativaRepository
    {
        int GetAlternativaCont(int IdQuestao);
        IEnumerable<Alternativa> GetAlternativasOrder(int IdQuestao);
    }
}
