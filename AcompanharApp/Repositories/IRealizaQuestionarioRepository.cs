using AcompanharApp.Models;
using System.Collections.Generic;

namespace AcompanharApp.Repositories
{
    public interface IRealizaQuestionarioRepository
    {

        bool IsQuestionario(int Code);

        Questionario Questionario(int Code);

        List<Questao> ListQuestoes(int Code);

    }
}
