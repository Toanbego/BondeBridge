using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondeBridge
{
    class Player
    {
        public string Name { get; set; }
        public int currentScore { get; set; }
        public int MyPrediction { get; set; }
        public int MyTricks{ get; set; }
        public int TotalScore{ get; set; }
        public bool isDealer { get; set; }
        public bool isLast { get; set; }


        public Player(string Name) 
        {
            this.Name = Name;
            this.currentScore = 0;
            this.TotalScore = 0;
        }

        public void CalculateScore(int currentRound)
        {
            if(MyPrediction == MyTricks)
            {
                currentScore = MyTricks + 10;
            }
            else { currentScore = MyTricks; }
            
            TotalScore += currentScore;
        }

    }
}
