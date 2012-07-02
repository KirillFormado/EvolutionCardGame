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

        private Mock<IPlayersRepository> playersRepositoryMock;
        private Mock<ICardsRepository> cardsRepositoryMock;

        private Dealer dealer;

        private const int playersCount = 3;
        private const int defaultCardsCount = 6;
        private const int fullDeck = 60;

        #endregion

        #region SetUp

        [SetUp]
        public void SetUp()
        {
            //Arrange
            var listPlayer = new List<Player>();
            for (int i = 0; i < playersCount; i++)
			    listPlayer.Add(new Player());     

            this.playersRepositoryMock = new Mock<IPlayersRepository>();
            this.playersRepositoryMock
                .Setup(p => p.GetPlayers())
                .Returns(listPlayer);
                    
            this.cardsRepositoryMock = new Mock<ICardsRepository>();
        }

        #endregion

        #region Tests

        [Test]
        public void DealCardsIfPlayerHasNotCardsInHandAndDeckIsEmpty_AllPlayersHaveDoNotGetCards()
        {
            //Arrange
            InitDealerEntity(0);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(0);
        }

        [Test]
        public void DealCardIfPlayerHasNotCardsInHandAndDeckIsNotEmpty_AllPlayersWillHaveDefaultCardsCountCardsInHand()
        {
            //Arrange
            int cardsCount = defaultCardsCount * playersCount;
            InitDealerEntity(cardsCount);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(defaultCardsCount);
        }

        [Test]
        public void DealCardIfPlayerHasCardsButHasNotAnimals_AllPlayersWillHaveDefaultCardsCountPlusOneCard()
        {
            //Arrange
            int cardsCount = fullDeck;
            InitDealerEntity(cardsCount);
            //first delivery cards
            dealer.DealCards();

            //Act
            dealer.DealCards();
            
            //Assert
            int expected = defaultCardsCount + 1;
            CardsCountAssert(expected);
        }

        [Test]
        public void DealCardIfPlayerHasCardsAndHasAnimals_AllPlayersWillHaveDefaultCardsCountPlusOneCardPlusCountOfAnimalsOnHand()
        {
            //Arrange
            int cardsCount = fullDeck;
            InitDealerEntity(cardsCount);
            //first delivery cards
            dealer.DealCards();
            GetPlayersAnimals();

            //Act
            dealer.DealCards();

            //Assert
            var players = dealer.Players;
            var animalCount = 1;
            foreach (var player in players)
            {
                for (int i = 0; i < animalCount; i++)
                {
                    int expected = defaultCardsCount + 1 + animalCount;
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
            this.InitDealerEntity(cardsCount);

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(1);
        }

        [Test]
        public void DealCardsPlayersHaveAnimals_()
        {
            //Arrange
            int cardsCount = playersCount * 3;
            InitDealerEntity(cardsCount);
            GetPlayersAnimals();

            //Act
            dealer.DealCards();

            //Assert
            CardsCountAssert(3);
        }

        #endregion

        #region  Helper Methods
        
        private void InitDealerEntity(int cardsCount)
        {
            var cardsList = new List<Card>();
            for (int i = 0; i < cardsCount; i++)
                cardsList.Add(new Card());

            this.cardsRepositoryMock.Setup(c => c.GetCards()).Returns(cardsList);
            this.dealer = new Dealer(playersRepositoryMock.Object, cardsRepositoryMock.Object);
        }

        private void GetPlayersAnimals()
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

        private void CardsCountAssert(int expectedCards)
        {
            var players = dealer.Players;
            foreach (var player in players)
                Assert.AreEqual(expectedCards, player.Cards.Count);    
        }

        #endregion
    }
}
