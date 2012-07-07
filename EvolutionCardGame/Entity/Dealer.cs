using System.Collections.Generic;
using System.Linq;
using ShuHaRi.EvolutionCardGame.Interfaces;

namespace ShuHaRi.EvolutionCardGame.Entity
{
    public class Dealer : IDealer
    {
        private const int defaultCardsCount = 6;
        private const int bonus = 1;
        private readonly IEnumerable<Player> players;

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public Dealer(IPlayersRepository playersRepository)
        {
            this.players = playersRepository.GetPlayers();
        }

        public void DealCards(ICardsDeck cardsDeck)
        {
            //Need to collect cards for each player
            var dictionary = this.Players.ToDictionary(player => player, player => new List<Card>());
            //Need to observe when players get all needed cards
            var playerList = new List<Player>(Players);

            while (playerList.Count > 0)
            {
                foreach (var player in this.Players)
                {
                    if (playerList.Contains(player))
                    {
                        var cardsList = dictionary[player];
                        if (!cardsDeck.IsDeckEmpty()
                            && cardsList.Count < this.CalculateNumberOfNeedeCards(player))
                        {
                            GetCardFromDeck(cardsList, cardsDeck);
                        }
                        else
                        {
                            playerList.Remove(player);
                        }
                    }
                }
            }

            foreach (var player in this.Players)
            {
                player.Cards.AddRange(dictionary[player]);
            }
        }

        private void GetCardFromDeck(List<Card> cardList, ICardsDeck cardsDeck)
        {
            cardList.Add(cardsDeck.Pop());    
        }

        private int CalculateNumberOfNeedeCards(Player player)
        {
            var playerCardsCount = player.Cards.Count;

            if (playerCardsCount == 0)
                return defaultCardsCount;

            if (playerCardsCount > 0)
                return this.CardsCountRule(player.Animals.Count);

            return 0;
        }

        private int CardsCountRule(int animalsCount)
        {
            return animalsCount + bonus;
        }
    }
}
