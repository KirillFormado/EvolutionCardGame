using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Moq;
using ShuHaRi.EvolutionCardGame.Entity;
using ShuHaRi.EvolutionCardGame.Interfaces;

namespace ShuHaRi.EvolutionCardGameTests
{
    [TestFixture]
    public class DealerDealCardsTests
    {
        #region Fields

        private const int playersCount = 3;
        private const int defaultCardsCount = 6;
        private const int bonus = 1;
        private const int fullDeck = 60;

        #endregion

        #region Tests

        [Test]
        public void DealCardsIfPlayerHasNotCardsInHandAndDeckIsEmpty_AllPlayersHaveDoNotGetCards()
        {
            //Arrange
            var dealer = this.BuildDealer();
            var cardsDeck = this.BuildCardsDeck(0);

            //Act
            dealer.DealCards(cardsDeck);

            //Assert
            PlayersCardsCountAssert(0, dealer);
            CardsDeckCardsCountAssert(0, cardsDeck);
        }

        [Test]
        public void DealCardIfPlayerHasNotCardsInHandAndDeckIsNotEmpty_AllPlayersWillHaveDefaultCardsCountCardsInHand()
        {
            //Arrange
            int cardsCount = defaultCardsCount * playersCount;
            var dealer = this.BuildDealer();
            var cardsDeck = this.BuildCardsDeck(cardsCount);

            //Act
            dealer.DealCards(cardsDeck);

            //Assert
            PlayersCardsCountAssert(defaultCardsCount, dealer);
            CardsDeckCardsCountAssert(0, cardsDeck);
        }

        [Test]
        public void DealCardIfPlayerHasCardsButHasNotAnimals_AllPlayersWillHaveDefaultCardsCountPlusOneCard()
        {
            //Arrange
            int cardsCount = fullDeck;
            var dealer = this.BuildDealer();
            var cardsDeck = this.BuildCardsDeck(cardsCount);
            //first delivery cards
            dealer.DealCards(cardsDeck);

            //Act
            dealer.DealCards(cardsDeck);
            
            //Assert
            int expectedPlayersCards = defaultCardsCount + 1;
            PlayersCardsCountAssert(expectedPlayersCards, dealer);
            int expectedCards = fullDeck - defaultCardsCount * playersCount - playersCount;
            CardsDeckCardsCountAssert(expectedCards, cardsDeck);
        }

        [Test]
        public void DealCardIfPlayerHasCardsAndHasAnimals_AllPlayersWillHaveDefaultCardsCountPlusBonusPlusCountOfAnimalsOnHand()
        {
            //Arrange
            int cardsCount = fullDeck;
            var dealer = this.BuildDealer();
            var cardsDeck = this.BuildCardsDeck(cardsCount);
            //first delivery cards
            dealer.DealCards(cardsDeck);
            BuildPlayersWithAnimals(dealer);

            //Act
            dealer.DealCards(cardsDeck);

            //Assert
            var players = dealer.Players;
            int animalCount = 1;
            int expectedAnimalsCount = 0;
            foreach (var player in players)
            {
                for (int i = 0; i < animalCount; i++)
                {
                    int expected = defaultCardsCount + bonus + animalCount;
                    Assert.AreEqual(expected, player.Cards.Count);
                }
                expectedAnimalsCount += animalCount;
                animalCount++;
            }

            int expectedCardsCount = cardsCount - defaultCardsCount*playersCount - expectedAnimalsCount - playersCount;

            CardsDeckCardsCountAssert(expectedCardsCount, cardsDeck);
        }

        [Test]
        public void DealCardsInDeckOnlyOneCardOnOnePlayer_AllPlayerMustHaveOneCard()
        {
            //Arrange
            var cardsCount = playersCount;
            var dealer = this.BuildDealer();
            var cardsDeck = this.BuildCardsDeck(cardsCount);

            //Act
            dealer.DealCards(cardsDeck);

            //Assert
            PlayersCardsCountAssert(1, dealer);
            CardsDeckCardsCountAssert(0, cardsDeck);
        }
    
        #endregion

        #region  Build Methods

        private IPlayersRepository BuildPlayersRepository()
        {
            var listPlayer = new List<Player>();
            for (int i = 0; i < playersCount; i++)
                listPlayer.Add(new Player());

            var playersRepositoryMock = new Mock<IPlayersRepository>();
            playersRepositoryMock
                .Setup(p => p.GetPlayers())
                .Returns(listPlayer);

            return playersRepositoryMock.Object;
        }

        private ICardsDeck BuildCardsDeck(int cardsCount)
        {
            var cardsRepositoryMock = new Mock<ICardsRepository>();
            var cardsList = new List<Card>();
            for (int i = 0; i < cardsCount; i++)
                cardsList.Add(new Card());

            cardsRepositoryMock.Setup(c => c.GetCards()).Returns(cardsList);

            ICardsDeck cardsDeck = this.CardsDeckFactory(cardsRepositoryMock.Object);

            return cardsDeck;
        }

        private ICardsDeck CardsDeckFactory(ICardsRepository cardsRepository)
        {
            return new CardsDeck(cardsRepository);
        }
        
        private Dealer BuildDealer()
        {
            var playersRepositoryMock = this.BuildPlayersRepository();
            var dealer = new Dealer(playersRepositoryMock);

            return dealer;
        }

        private void BuildPlayersWithAnimals(Dealer dealer)
        {
            var players = dealer.Players;
            int animalCount = 1;
            foreach (var player in players)
            {
                for (int i = 0; i < animalCount; i++)
                {
                    player.Animals.Add(new Animal());
                }
                animalCount++;
            }
        }

        #endregion

        #region Assert Methods

        private void PlayersCardsCountAssert(int expectedPlayerCards, Dealer dealer)
        {
            var players = dealer.Players;
            foreach (var player in players)
                Assert.AreEqual(expectedPlayerCards, player.Cards.Count);
        }

        private void CardsDeckCardsCountAssert(int expectedCardsCount, ICardsDeck cardsDeck)
        {
            Assert.AreEqual(expectedCardsCount, cardsDeck.CardsCount());
        }

        #endregion
    }
}
