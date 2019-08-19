using FlipFlop.Interface_WPF.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FlipFlop.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateDeck()
        {
            //Arrange
            Deck deck = new Deck();

            //Act

            //Assert
            Assert.AreEqual(52, deck.CardList.Count);
            Assert.AreEqual(4, deck.CardList.Where(x => x.Value == 13).Count());
        }

        //[TestMethod]
        //public void TestCreateDeck()
        //{
        //    //Arrange
        //    Deck deck = new Deck();

        //    //Act

        //    //Assert
        //    Assert.AreEqual(52, deck.CardList.Count);
        //    Assert.AreEqual(4, deck.CardList.Where(x => x.Value == 13).Count());
        //}
    }
}
