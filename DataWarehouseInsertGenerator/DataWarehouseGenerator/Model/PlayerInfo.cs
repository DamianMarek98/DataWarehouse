using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVGenerator.Model
{
    public class PlayerInfo
    {
        public int playerId { get; set; }
        public bool wereVariablesPaid { get; set; }
        public double variablesAmount { get; set; }
        public System.DateTime deadline { get; set; }
        public System.DateTime payday { get; set; }

        public PlayerInfo(int playerId, bool wereVariablesPaid, double variablesAmount, System.DateTime deadline, System.DateTime payday)
        {
            this.playerId = playerId;
            this.wereVariablesPaid = wereVariablesPaid;
            this.variablesAmount = variablesAmount;
            this.deadline = deadline;
            this.payday = payday;
        }
    }
}
