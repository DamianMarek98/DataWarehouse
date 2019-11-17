using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVGenerator.Model
{
    public class ContractVariables
    {
        public int playerId { get; set; }
        public int agentId { get; set; }
        public double commission { get; set; }
        public double salary { get; set; }
        public System.DateTime signingDay { get; set; }
        public System.DateTime expiryDay { get; set; }

        public ContractVariables(int playerId, int agentId, double commission, double salary, System.DateTime signingDay, System.DateTime expiryDay)
        {
            this.playerId = playerId;
            this.agentId = agentId;
            this.commission = commission;
            this.salary = salary;
            this.signingDay = signingDay;
            this.expiryDay = expiryDay;
        }
    }
}

