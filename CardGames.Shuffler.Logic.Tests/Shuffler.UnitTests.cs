using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CardGames.Shuffler.Logic.Tests
{
    [TestClass]
    public class ShufflerUnitTests
    {
        [TestMethod]
        public void ShuffleShouldShuffleTwoDecksOnTuesdays()
        {
            // create the ShimsContext in order to use Shims later
            using (ShimsContext.Create())
            {
                // arrange
                // intercept the call to System.DateTime.Now and always return a date
                // that we're sure falls on a Tuesday (09/12/2017 is a Tuesday)
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2017, 09, 12);
                var shuffler = new Shuffler();

                // act
                var cards = shuffler.Shuffle();

                // assert
                Assert.AreEqual(104, cards.Count);
            }
        }

        [TestMethod]
        public void ShuffleShouldShuffleOneDeckOnOtherDays()
        {
            // create the ShimsContext in order to use Shims later
            using (ShimsContext.Create())
            {
                // arrange
                System.Fakes.ShimDateTime.NowGet = () => new DateTime(2017, 09, 13);
                var shuffler = new Shuffler();

                // act
                var cards = shuffler.Shuffle();

                // assert
                Assert.AreEqual(52, cards.Count);
            }
        }

        [TestMethod]
        public void ShuffleShouldShuffleDeck()
        {
            // arrange
            var position = 53;
            // setup the Random stub so that when Next is called with two
            // integers (min value and max value) we always return a number
            // we know

            // what we're going to do here is go backward through the unshuffled
            // deck so the "shuffled" deck that we end up with will just be an
            // inversion of the unshuffled deck (KS will be first and AH will be last)
            var randomStub = new System.Fakes.StubRandom
            {
                NextInt32Int32 = (minValue, maxValue) =>
                {
                    if (position == 1)
                    {
                        position = 53;
                    }
                    position--;
                    return position;
                }
            };

            var unshuffledDeck = new List<string>
            {
                "AH", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H",
                "JH", "QH", "KH", "AD", "2D", "3D", "4D", "5D", "6D", "7D",
                "8D", "9D", "10D", "JD", "QD", "KD", "AC", "2C", "3C", "4C",
                "5C", "6C", "7C", "8C", "9C", "10C", "JC", "QC", "KC", "AS",
                "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "10S", "JS",
                "QS", "KS"
            };

            var shuffler = new Shuffler { Random = randomStub };

            // act
            var cards = shuffler.Shuffle();

            // assert
            var j = 51;
            for (var i = 0; i < 51; i++)
            {
                Assert.AreEqual(unshuffledDeck[i], cards[j]);
                j--;
            }
        }
    }
}