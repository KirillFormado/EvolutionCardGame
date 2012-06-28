using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuHaRi.EvolutionCardGame.Entity;

namespace ShuHaRi.EvolutionCardGame.Interfaces
{
    public interface ICardsRepository
    {
        IEnumerable<Card> GetCards();
    }
}
