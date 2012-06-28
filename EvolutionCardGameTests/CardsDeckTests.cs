using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShuHaRi.EvolutionCardGame.Entity;
using ShuHaRi.EvolutionCardGame.Interfaces;

namespace ShuHaRi.EvolutionCardGameTests
{
    [TestFixture]
    public class CardsDeckTests
    {
        private  Mock<ICardsRepository> cardsRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            cardsRepositoryMock = new Mock<ICardsRepository>();
        }

        #region Tests

        [Test]
        public void InitializeCardsDeck_CardsDeckIsEmpty()
        {
            //Act
            var cardsDeck = new CardsDeck(cardsRepositoryMock.Object);

            //Assert
            Assert.AreEqual(0, cardsDeck.CardsCount());
        }

        [Test]
        public void InitializeCardsDeck_CardDeckHasCards()
        {
            //Arrange
            int cardsCount = 10;
            this.InitCardsDeck(cardsCount);

            //Act
            var cardsDeck = new CardsDeck(cardsRepositoryMock.Object);

            //Assert
            Assert.AreEqual(cardsCount, cardsDeck.CardsCount());
        }

        [Test]
        public void GetSomeCardsFromCardsDeck_ReturnNewCardsCount()
        {
            //Arrange
            int cardsCount = 10;
            int popCardsCount = cardsCount - 2;
            int expectedCardsCount = cardsCount - popCardsCount;
            this.InitCardsDeck(cardsCount);
            var cardsDeck = new CardsDeck(cardsRepositoryMock.Object);

            //Act
            for (int i = 0; i < popCardsCount; i++)
            {
                cardsDeck.Pop();
            }

            //Assert
            Assert.AreEqual(expectedCardsCount, cardsDeck.CardsCount());
        }

        [Test]
        public void GetAllCardsFromCardDeckAndTryGetOneMore_ReturnNull()
        {
            //Arrange
            int cardsCount = 10;
            this.InitCardsDeck(cardsCount);
            var cardsDeck = new CardsDeck(cardsRepositoryMock.Object);

            //Act
            for (int i = 0; i < cardsCount; i++)
            {
                cardsDeck.Pop();
            }

            //Assert
            Assert.AreEqual(0, cardsDeck.CardsCount());
            Assert.IsNull(cardsDeck.Pop());
        }

        #endregion

        #region Helper Methods

        private void InitCardsDeck(int cardsCount)
        {
              cardsRepositoryMock
                .Setup(c => c.GetCards())
                .Returns(() =>
                             {
                                 var list = new List<Card>();
                                 for (int i = 0; i < cardsCount; i++)
                                 {
                                     list.Add(new Card());
                                 }
                                 return list;
                             });
        }

        #endregion
    }
}
