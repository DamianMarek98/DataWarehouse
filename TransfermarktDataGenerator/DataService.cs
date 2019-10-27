using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransfermarktDataGenerator.Model;

namespace TransfermarktDataGenerator
{
    class DataService
    {
        public static void addAgent(TAgent newAgent)
        {
            var dbContext = new DataWarehousesProjectEntities();
            int maxID;
            if (dbContext.Agent.Count() != 0) maxID = dbContext.Agent.Max(x => x.Id);
            else maxID = -1;
            Agent agent = new Agent();
            agent.Id = maxID + 1;
            agent.Imię = newAgent.Imię;
            agent.Nazwisko = newAgent.Nazwisko;
            dbContext.Agent.Add(agent);
            dbContext.SaveChanges();
        }
    }
}
