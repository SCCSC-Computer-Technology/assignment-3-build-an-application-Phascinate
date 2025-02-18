using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatesDatabaseClassLibrary
{
    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int Population { get; set; }
        public string FlagDescription { get; set; }
        public string StateFlower { get; set; }
        public string StateBird { get; set; }
        public string Colors { get; set; }
        public string LargestCities { get; set; }
        public string StateCapital { get; set; }
        public decimal MedianIncome { get; set; }
        public decimal ComputerJobsPercent { get; set; }
    }
}

