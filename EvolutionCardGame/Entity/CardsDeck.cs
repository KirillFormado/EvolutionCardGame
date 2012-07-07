using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuHaRi.EvolutionCardGame.Interfaces;

namespace ShuHaRi.EvolutionCardGame.Entity
{
    public class CardsDeck : ICardsDeck
    {
        private readonly Stack<Card> cardsRepository;

        public CardsDeck(ICardsRepository cardsRepository)
        {
            this.cardsRepository = new Stack<Card>(cardsRepository.GetCards());
        }

        public int CardsCount()
        {
            return cardsRepository.Count;
        }

        public Card Pop()
        {
            if (IsDeckEmpty())
                return null;

            return cardsRepository.Pop();
        }

        public bool IsDeckEmpty()
        {
            return !(cardsRepository.Count > 0);
        }
    }
}
