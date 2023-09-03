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

        public bool isDealer { get; set; }

        public bool isLast { get; set; }


        public Player(string Name) 
        {
            this.Name = Name;
            this.currentScore = 0;
        }
    }
}
