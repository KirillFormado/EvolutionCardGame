using System.Collections.Generic;

namespace ShuHaRi.EvolutionCardGame.Entity
{
    public class Player
    {
        private List<Card> cards;
        private List<Animal> animals;

        public List<Card> Cards
        {
            get
            {
                if (this.cards == null)
                {
                    this.cards = new List<Card>();
                }
                return this.cards;
            }
        }

        public List<Animal> Animals
        {
            get
            {
                if (this.animals == null)
                {
                    this.animals = new List<Animal>();
                }
                return this.animals;
            }
        }
    }
}
