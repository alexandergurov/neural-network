using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Synapse
    {
        private double input;
        private double weight = new Random().NextDouble();
        

        public double Input { get => input; set => input = value; }
        public double Weight { get => weight; set => weight = value; }
        
    }
}
