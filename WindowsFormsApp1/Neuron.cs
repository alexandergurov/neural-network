using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Neuron
    {
        public double alpha = 1;

        private Synapse[] synapses;
        public double output;

        private double delta = 1;
        public double Delta { get => delta; set => delta = value; }

        internal Synapse[] Synapses { get => synapses; set => synapses = value; }

        public Neuron(int synapsesCount) {
            Synapses = new Synapse[synapsesCount];
            for (int i = 0; i < synapsesCount; i++) {
                Synapses[i] = new Synapse();
            }
        }

        public double CalculateOutput() {
            double summ = 0;
            for (int i = 0; i < Synapses.Length; i++) {
                summ += Synapses[i].Input * Synapses[i].Weight;
            }
            output = (double) 1 / (double)(1 + Math.Exp(- 2 * alpha * summ));
            return output;
        }

        public void setSynapsesInputs(double[] inputs) {
            if (inputs.Length == synapses.Length)
            {
                for (int i = 0; i < synapses.Length; i++) {
                    synapses[i].Input = inputs[i];
                }
            }
            else throw new Exception("Wrong input lenght for neuron");
        }
    }
}