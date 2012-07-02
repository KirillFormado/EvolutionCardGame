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
            var dealer = this.BuildDealer(0);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(0, dealer);
        }

        [Test]
        public void DealCardIfPlayerHasNotCardsInHandAndDeckIsNotEmpty_AllPlayersWillHaveDefaultCardsCountCardsInHand()
        {
            //Arrange
            int cardsCount = defaultCardsCount * playersCount;
            var dealer = this.BuildDealer(cardsCount);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(defaultCardsCount, dealer);
        }

        [Test]
        public void DealCardIfPlayerHasCardsButHasNotAnimals_AllPlayersWillHaveDefaultCardsCountPlusOneCard()
        {
            //Arrange
            int cardsCount = fullDeck;
            var dealer = this.BuildDealer(cardsCount);
            //first delivery cards
            dealer.DealCards();

            //Act
            dealer.DealCards();
            
            //Assert
            int expected = defaultCardsCount + 1;
            CardsCountAssert(expected, dealer);
        }

        [Test]
        public void DealCardIfPlayerHasCardsAndHasAnimals_AllPlayersWillHaveDefaultCardsCountPlusBonusPlusCountOfAnimalsOnHand()
        {
            //Arrange
            int cardsCount = fullDeck;
            var dealer = this.BuildDealer(cardsCount);
            //first delivery cards
            dealer.DealCards();
            BuildPlayersWithAnimals(dealer);

            //Act
            dealer.DealCards();

            //Assert
            var players = dealer.Players;
            var animalCount = 1;
            foreach (var player in players)
            {
                for (int i = 0; i < animalCount; i++)
                {
                    int expected = defaultCardsCount + bonus + animalCount;
                    Assert.AreEqual(expected, player.Cards.Count);
                }
                animalCount++;
            }
        }

        [Test]
        public void DealCardsInDeckOnlyOneCardOnOnePlayer_AllPlayerMustHaveOneCard()
        {
            //Arrange
            var cardsCount = playersCount;
            var dealer = this.BuildDealer(cardsCount);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(1, dealer);
        }

        [Ignore("Temp Ignore")]
        [Test]
        public void DealCardsPlayersHaveAnimals_Return()
        {
            //Arrange
            int cardsCount = playersCount * 3;
            var dealer = this.BuildDealer(cardsCount);
            BuildPlayersWithAnimals(dealer);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(3, dealer);
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

        private ICardsRepository BuildCardsRepository(int cardsCount)
        {
            var cardsRepositoryMock = new Mock<ICardsRepository>();
            var cardsList = new List<Card>();
            for (int i = 0; i < cardsCount; i++)
                cardsList.Add(new Card());

            cardsRepositoryMock.Setup(c => c.GetCards()).Returns(cardsList);

            return cardsRepositoryMock.Object;
        }
        
        private Dealer BuildDealer(int cardsCount)
        {
            var playersRepositoryMock = this.BuildPlayersRepository();
            var cardsRepositoryMock = this.BuildCardsRepository(cardsCount);

            var dealer = new Dealer(playersRepositoryMock, cardsRepositoryMock);

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

        private void CardsCountAssert(int expectedCards, Dealer dealer)
        {
            var players = dealer.Players;
            foreach (var player in players)
                Assert.AreEqual(expectedCards, player.Cards.Count);
        }

        #endregion
    }
}
