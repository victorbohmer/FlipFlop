using FlipFlop.Interface_WPF;
using FlipFlop.Interface_WPF.AI;
using FlipFlop.Interface_WPF.GameClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FlipFlop.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDeckCreation()
        {
            //Arrange
            Deck deck = new Deck();

            //Act

            //Assert
            Assert.AreEqual(52, deck.CardList.Count);
            Assert.AreEqual(4, deck.CardList.Where(x => x.Value == 13).Count());
        }
        [TestMethod]
        public void TestBoardCreation()
        {
            //Arrange
            Deck deck = new Deck();
            Board board = new Board(deck);

            //Act

            //Assert
            Assert.AreEqual(9, board.Spaces.Count);
            Assert.AreEqual(0, board.Spaces.Where(x => x.Card != null).Count());
        }
        [TestMethod]
        public void TestPlayerCreation()
        {
            //arrange
            Deck deck = new Deck();
            Player player = new Player(deck, 2);

            //act
            player.DrawNewHand();

            //assert
            Assert.AreEqual(5, player.Hand.Count);
            Assert.AreEqual(5, player.Hand.Where(x => x.Card != null).Count());

        }

        [TestMethod]
        public void TestAIPlayerCreation()
        {
            //arrange
            Deck deck = new Deck();
            Board board = new Board(deck);
            Player player = new Player(deck, 2);
            AIPlayer aiPlayer = new Aziraphale(board, player);
            player.DrawNewHand();
            PlayerCardSpace highestCard = player.Hand.OrderByDescending(x => x.Card.Value).First();

            //act

            PlayerCardSpace cardSpaceToPlayFrom = aiPlayer.SelectCardToPlay();
            Card cardToPlay = cardSpaceToPlayFrom.TakeCard();


            //assert
            Assert.AreEqual(4, aiPlayer.Player.Hand.Where(x => x.Card != null).Count());
            Assert.IsTrue(aiPlayer.Player.Hand.Contains(highestCard));

        }
    }
}
