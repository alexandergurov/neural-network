using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Network
    {
        private Neuron[][] network;

        public int width;
        public int height;        

        public double[] dataSetX;
        public double[] dataSetY;
        public double error;

        public double eta;
        public int epochCount;

        public double[] resultDataSetY;

        public Network(int countOfInputs, int innerLayersCount, int innerLayersHeight, int countOfOutputs, double eta = 0.7, int epochCount = 10)
        {
            this.eta = eta;
            this.epochCount = epochCount;
            this.width = innerLayersCount;
            this.height = innerLayersHeight;            
            network = new Neuron[innerLayersCount + 2][];

            // creation of first layer
            network[0] = new Neuron[countOfInputs];
            for (int i = 0; i < countOfInputs; i++) {
                network[0][i] = new Neuron(1);
            }
            // creation of hidden layers
            for (int w = 1; w < innerLayersCount + 1; w++){
                network[w] = new Neuron[innerLayersHeight];
                for (int j = 0; j < innerLayersHeight; j++){
                    network[w][j] = new Neuron(network[w - 1].Length);
                }
            }
            // creation of output layers
            network[innerLayersCount + 1] = new Neuron[countOfOutputs];
            for (int i = 0; i < countOfOutputs; i++){
                network[innerLayersCount + 1][i] = new Neuron(network[innerLayersCount].Length);
            }
        }

        public double[] calculate(double[] input) {
            if (input.Length != network[0].Length) {
                throw new Exception("Incorrect input lenght for this network configuration");
            }
            // set inputs on first layer synapses and calc outputs
            for (int i = 0; i < network[0].Length; i++) {
                network[0][i].Synapses[0].Input = input[i];
                network[0][i].CalculateOutput();
            }
            // calc other network outputs
            for (int w = 1; w < width + 2; w++){
                for (int h = 0; h < network[w].Length; h++){                    
                    network[w][h].setSynapsesInputs(this.getLayerOutputs(w-1));                    
                    network[w][h].CalculateOutput();
                }
            }            
            return this.getLayerOutputs(network.Length - 1);
        }

        private double[] getLayerOutputs(int layerID)
        {
            double[] outputs = new double[network[layerID].Length];
            for (int y = 0; y < network[layerID].Length; y++){
                outputs[y] = network[layerID][y].output;
            }
            return outputs;
        }

        public void setData()
        {
            dataSetX = new double[1000];
            dataSetY = new double[1000];
            for (int i = 0; i < 1000; i++)
            {
                dataSetX[i] = i * 2 * Math.PI / 1000;
                dataSetY[i] = 0.5 + Math.Sin(dataSetX[i]) / 2;
            }
        }

        public void teach() {            
            double alpha = 1;
            resultDataSetY = new double[1000];
            //Random rand = new Random();
            for (int epochStep = 0; epochStep < epochCount; epochStep++)
            {
                for (int dataSetID = 0; dataSetID < dataSetY.Length; dataSetID++) {
                //int dataSetID = rand.Next(0,999);
                    calculate(new double[] { dataSetX[dataSetID] }); // @TODO CHEAT!
                    resultDataSetY[dataSetID] = network[width+1][0].output;
                    for (int w = width + 1; w >=1; w--) {
                        for (int h = 0; h < network[w].Length; h++){
                            double delta = 0;
                            if (w == width + 1) // last layer
                            {
                                delta = - 2 * alpha * network[w][h].output * (1 - network[w][h].output)
                                    * (dataSetY[dataSetID] - network[w][h].output);  // @TODO CHEAT!
                                for (int sID = 0; sID < height; sID++)
                                {
                                    network[w][h].Synapses[sID].Weight = network[w][h].Synapses[sID].Weight -
                                        eta * delta * network[w][h].Synapses[sID].Input;
                                }
                            }
                            else // other layers
                            {
                                double errorSum = 0;
                                for (int nextLayerHeightID = 0; nextLayerHeightID < network[w+1].Length; nextLayerHeightID++)
                                {
                                    errorSum += network[w + 1][nextLayerHeightID].Delta * network[w + 1][nextLayerHeightID].Synapses[h].Weight;
                                }
                                delta = 2 * alpha * network[w][h].output * (1 - network[w][h].output) * errorSum;
                            }
                            network[w][h].Delta = delta;
                            for (int sID = 0; sID < network[w][h].Synapses.Length; sID++) {
                                network[w][h].Synapses[sID].Weight = network[w][h].Synapses[sID].Weight -
                                    eta * delta * network[w][h].Synapses[sID].Input;
                            }
                        }
                    }                    
                }
            }
        }        

        private double calculateError() {
            double error = 0;
            for (int i = 0; i < 1000; i++) {
                error += Math.Pow(resultDataSetY[i] - dataSetY[i], 2);
            }
            return error/2;
        }

        public string ToString() {
            return "Network conf: width=" + width.ToString() + ", height=" + height.ToString()
                + "\r\nTeaching conf: eta=" + eta.ToString() + ", epochCount=" + epochCount.ToString();
                ;
        }
        
    }
}
