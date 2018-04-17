using ECore.Web.BLService;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ECore.Test
{
    public class AlgorithmSMTest
    {
        /// <summary>
        /// Test calculation group repatition
        /// </summary>
        [Fact]
        public void CalcGroupRepetitionTest()
        {
            // Arrang
            int group = 0;
            double ef = 3.0;

            // Act
            int res = AlgorithmSM.CalcGroupRepetition(group, ef);

            // Assert
            Assert.Equal(1, res);
        }

        [Fact]
        public void CalcIntervalTest()
        {
            // Arrang
            int group = 1;
            double ef = 3.0;

            // Act
           double res = AlgorithmSM.CalcInterval(group, ef);

            // Assert
            Assert.Equal(6, res);
        }

        [Fact]
        public void CalcIntervalErrorTest()
        {
            // Arrang
            int group = -1;
            double ef = 3.0;

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => AlgorithmSM.CalcInterval(group, ef));

            // Assert
            Assert.Contains("The number of repetitions must be 0 or higher", ex.Message);
        }
    }
}
