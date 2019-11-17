using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVGenerator.Model
{
    class PlayerContractFullInfo
    {
        public int playerId { get; set; }
        public int agentId { get; set; }
        public double commission { get; set; }
        public double salary { get; set; }
        public System.DateTime signingDay { get; set; }
        public System.DateTime expiryDay { get; set; }
        public bool wereVariablesPaid { get; set; }
        public double variablesAmount { get; set; }
        public System.DateTime deadline { get; set; }
        public System.DateTime payday { get; set; }

        public PlayerContractFullInfo(int playerId, bool wereVariablesPaid, double variablesAmount, System.DateTime deadline, System.DateTime payday,
            int agentId, double commission, double salary, System.DateTime signingDay, System.DateTime expiryDay)
        {
            this.playerId = playerId;
            this.wereVariablesPaid = wereVariablesPaid;
            this.variablesAmount = variablesAmount;
            this.deadline = deadline;
            this.payday = payday;
            this.agentId = agentId;
            this.commission = commission;
            this.salary = salary;
            this.signingDay = signingDay;
            this.expiryDay = expiryDay;
        }
    }
}
