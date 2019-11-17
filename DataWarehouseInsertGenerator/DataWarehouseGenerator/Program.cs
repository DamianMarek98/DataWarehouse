using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataWarehouseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertGenerator insertGenerator = new InsertGenerator(20);
            insertGenerator.Generate();
            
        }
    }
}
