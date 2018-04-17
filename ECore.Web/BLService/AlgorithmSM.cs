using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECore.Web.BLService
{
    public static class AlgorithmSM
    {
        
        /// <summary>
        /// Calculate interval by algorithm sm-2 in days
        /// </summary>
        /// <param name="numberOfRepeats"></param>
        /// <param name="ef"></param>
        /// <returns></returns>
        public static double CalcInterval(int groupRepetition, double ef)
        {
            if (groupRepetition < 0)
            {
                throw new ArgumentOutOfRangeException("The number of repetitions must be 0 or higher");
            }

            if (ef >= 3)
            {
                if (groupRepetition == (int)GroupRepetition.NoMemorized)
                {
                    return 1;
                }
                else if (groupRepetition == (int)GroupRepetition.AlmostMemorized)
                {
                    return 6;
                }
                else
                {
                    return Math.Ceiling((CalcInterval(groupRepetition - 1, ef) * ef));
                }
            }
            else
            {
                return 1;
            }                  
        }


        /// <summary>
        /// Calculate new e-factor by algorithm sm-2.
        /// </summary>
        /// <param name="oldef"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static double CalcNewEfactor(double efactor, int score)
        {

            if (score < 0 || score > 5)
            {
                throw new ArgumentOutOfRangeException("User score must be from 0 to 5");
            }

            efactor = efactor + (0.1 - (5 - score) * (0.08 + (5 - score) * 0.02));

            if (efactor < 3.0)
            {
                return 2.9;
            }

            return efactor;
        }


        /// <summary>
        /// Calculate group of repetition
        /// </summary>
        /// <param name="groupRepetition"></param>
        /// <param name="ef"></param>
        /// <returns></returns>
        public static int CalcGroupRepetition(int groupRepetition, double ef)
        {
            if (ef >= 3)
            {
                if (groupRepetition == (int)GroupRepetition.NoMemorized)
                {
                    return (int)GroupRepetition.AlmostMemorized;
                }
                else if (groupRepetition == (int)GroupRepetition.AlmostMemorized)
                {
                    return (int)GroupRepetition.Memorized;
                }
                else
                {
                    return (++groupRepetition);
                }
            }
            else
            {
                return 0;
            }           
        }
    }
}
