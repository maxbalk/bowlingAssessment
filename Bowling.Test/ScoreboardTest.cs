using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bowling.Test
{
    [TestClass]
    public class ScoreboardTest
    {

        [TestMethod]
        public void ValidPlayer()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            Assert.AreEqual(sut.Frames["asdf"].Count, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicatePlayerException))]
        public void DuplicatePlayerName()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.AddPlayer("asdf");
        }

        [TestMethod]
        public void StrikeTrue()
        {
            Scoreboard sut = new Scoreboard();
            List<int> frame = new List<int>() { 10 };
            Assert.IsTrue(sut.IsStrike(frame));
        }

        [TestMethod]
        public void StrikeFalse()
        {
            Scoreboard sut = new Scoreboard();
            List<int> frame = new List<int>() { 9,1 };
            Assert.IsFalse(sut.IsStrike(frame));
        }

        [TestMethod]
        public void SpareTrue()
        {
            Scoreboard sut = new Scoreboard();
            List<int> frame = new List<int>() { 9,1 };
            Assert.IsTrue(sut.IsSpare(frame));
        }

        [TestMethod]
        public void SpareFalse()
        {
            Scoreboard sut = new Scoreboard();
            List<int> frame = new List<int>() { 8,1 };
            Assert.IsFalse(sut.IsSpare(frame));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void RollInvalidPlayer()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("jkl;", 10, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void RollInvalidFrame()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 11);
        }

        [TestMethod]
        [ExpectedException(typeof(NumPinsException))]
        public void RollInvalidPins()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 11, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NumPinsException))]
        public void RollInvalidPinSum()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 6, 1);
            sut.Roll("asdf", 6, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRollException))]
        public void RollExceedsCount()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 1, 1);
            sut.Roll("asdf", 1, 1);
            sut.Roll("asdf", 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRollException))]
        public void RollExceedsCountLastFrame()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 1, 10);
            sut.Roll("asdf", 1, 10);
            sut.Roll("asdf", 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRollException))]
        public void RollTooManyAfterStrike()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 1);
            sut.Roll("asdf", 1, 1);
            sut.Roll("asdf", 1, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRollException))]
        public void RollExceedsStrikeBonus()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 10, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRollException))]
        public void RollExceedsSpareBonus()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 4, 10);
            sut.Roll("asdf", 6, 10);
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 10, 10);
        }

        [TestMethod]
        public void RollNormalPins()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 7, 3);
            sut.Roll("asdf", 1, 3);
            List<int> expected = new List<int>() { 7, 1 };
            Assert.IsTrue(sut.Frames["asdf"][2].SequenceEqual(expected));
        }

        [TestMethod]
        public void RollStrikeBonus()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 2, 10);
            sut.Roll("asdf", 2, 10);
            List<int> expected = new List<int>() { 10, 2, 2 };
            Assert.IsTrue(sut.Frames["asdf"][^1].SequenceEqual(expected));
        }

        [TestMethod]
        public void RollSpareBonus()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 2, 10);
            sut.Roll("asdf", 8, 10);
            sut.Roll("asdf", 2, 10);
            List<int> expected = new List<int>() { 2, 8, 2 };
            Assert.IsTrue(sut.Frames["asdf"][^1].SequenceEqual(expected));
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void ScoreInvalidFrame()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.ScoreFrame("asdf", 11);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ScoreInvalidPlayer()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.ScoreFrame("jkl;", 10);
        }

        [TestMethod]
        public void ScoreEmptyFrame()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            Assert.AreEqual(sut.ScoreFrame("asdf", 1), 0);
        }

        [TestMethod]
        public void ScoreNormalFrame()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 2, 1);
            sut.Roll("asdf", 4, 1);
            Assert.AreEqual(sut.ScoreFrame("asdf", 1), 6);
        }

        [TestMethod]
        public void ScoreStrike()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 1);
            sut.Roll("asdf", 4, 2);
            sut.Roll("asdf", 6, 2);
            Assert.AreEqual(sut.ScoreFrame("asdf", 1), 20);
        }

        [TestMethod]
        public void ScoreSpare()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 4, 1);
            sut.Roll("asdf", 6, 1);
            sut.Roll("asdf", 6, 2);
            Assert.AreEqual(sut.ScoreFrame("asdf", 1), 16);
        }

        [TestMethod]
        public void ScoreBonusStrike()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 4, 10);
            sut.Roll("asdf", 6, 10);
            Assert.AreEqual(sut.ScoreFrame("asdf", 10), 20);
        }

        [TestMethod]
        public void ScoreBonusSpare()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.Roll("asdf", 5, 10);
            sut.Roll("asdf", 5, 10);
            sut.Roll("asdf", 3, 10);
            Assert.AreEqual(sut.ScoreFrame("asdf", 10), 13);
        }

        [TestMethod]
        public void ScoreThreeHundred()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            for (int i=1; i<11; i++)
            {
                sut.Roll("asdf", 10, i);
            }
            sut.Roll("asdf", 10, 10);
            sut.Roll("asdf", 10, 10);
            int totalScore = 0;
            for (int i=1; i<11; i++)
            {
                totalScore += sut.ScoreFrame("asdf", i);
            }
            Assert.AreEqual(totalScore, 300);
        }

        [TestMethod]
        public void ScoreAllSparesNoStrikes()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            for (int i = 1; i < 11; i++)
            {
                sut.Roll("asdf", 9, i);
                sut.Roll("asdf", 1, i);
            }
            sut.Roll("asdf", 9, 10);
            int totalScore = 0;
            for (int i = 1; i < 11; i++)
            {
                totalScore += sut.ScoreFrame("asdf", i);
            }
            Assert.AreEqual(totalScore, 190);
        }

        [TestMethod]
        public void ShowpartialGameScoreboard()
        {
            Scoreboard sut = new Scoreboard();
            sut.AddPlayer("asdf");
            sut.AddPlayer("zxcv");
            sut.Roll("asdf", 9, 1);
            sut.Roll("asdf", 0, 1);
            sut.Roll("zxcv", 6, 1);
            sut.Roll("zxcv", 0, 1);
            sut.Roll("asdf", 9, 2);
            sut.Roll("asdf", 0, 2);
            sut.Roll("zxcv", 6, 2);
            sut.Roll("zxcv", 0, 2);
            Console.WriteLine(sut.ShowScoreboard());
        }


    }
}
