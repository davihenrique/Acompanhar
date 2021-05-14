using Acompanhar.Models;
using System.Collections.Generic;

namespace Acompanhar.Repositories
{
    public interface IRealizaQuestionarioRepository
    {

        bool IsQuestionario(int Code);

        Questionario Questionario(int Code);

        List<Questao> ListQuestoes(int Code);

    }
}
