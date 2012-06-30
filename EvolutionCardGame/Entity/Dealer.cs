using System.Collections.Generic;
using System.Linq;
using ShuHaRi.EvolutionCardGame.Interfaces;

namespace ShuHaRi.EvolutionCardGame.Entity
{
    public class Dealer : IDealer
    {
        private const int defaultCardsCount = 6;
        private readonly IEnumerable<Player> players;
        private readonly CardsDeck cardsDeck;

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public Dealer(IPlayersRepository playersRepository, CardsDeck cardsDeck)
        {
            this.players = playersRepository.GetPlayers();
            this.cardsDeck = cardsDeck;
        }

        public void DealCards()
        {
            var dictionary = this.Players.ToDictionary(player => player, player => new List<Card>());
            var playerList = new List<Player>(Players);

            while (playerList.Count > 0)
            {
                foreach (var player in this.Players)
                {
                    if (!this.cardsDeck.IsDeckEmpty() && dictionary[player].Count < this.CalculateNumberOfNeedeCards(player))
                    {
                        GetCardFromDeck(dictionary[player]);
                    }
                    else
                    {
                        playerList.Remove(player);
                    }
                }
            }

            foreach (var player in players)
            {
                player.Cards.AddRange(dictionary[player]);
            }
        }

        private void GetCardFromDeck(List<Card> cardList)
        {
            cardList.Add(this.cardsDeck.Pop());    
        }

        private int CalculateNumberOfNeedeCards(Player player)
        {
            var playerCardsCount = player.Cards.Count;
            if (playerCardsCount == 0)
                return defaultCardsCount;

            if (playerCardsCount > 0)
                return this.CardsCount(player.Animals.Count);

            return 0;
        }

        private int CardsCount(int animalsCount)
        {
            return animalsCount + 1;
        }


    }
}
