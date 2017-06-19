using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void programToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NeuronFunction.Series.Clear();
            NeuronFunction.Series.Add("Activation Function");
            NeuronFunction.Series["Activation Function"].ChartType = SeriesChartType.Line;
            for (double i = -10; i < 10; i = i + 0.1 ){
                DataPoint Dp = new DataPoint(i , 1 / (1 + Math.Exp(-2 * 1 * i)));
                NeuronFunction.Series["Activation Function"].Points.Add(Dp);
            }

            Network network = new Network(1, 1, 10, 1, 0.7, 1 );            
            fillData(network, ref chart1, textBox1);

            Network network2 = new Network(1, 1, 10, 1, 0.7, 10);
            fillData(network2, ref chart2, textBox2);

            Network network3 = new Network(1, 1, 10, 1, 0.2, 10);
            fillData(network3, ref chart3, textBox3);

            Network network4 = new Network(1, 2, 10, 1, 0.2, 10);
            fillData(network4, ref chart4, textBox4);
        }

        private void fillData(Network net, ref Chart chart, TextBox text) {
            net.setData();
            chart.Series.Clear();
            chart.Series.Add("Sin");
            chart.Series["Sin"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < 1000; i++){
                DataPoint Dp = new DataPoint(net.dataSetX[i], net.dataSetY[i]);
                chart.Series["Sin"].Points.Add(Dp);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            net.teach();
            watch.Stop();

            chart.Series.Add("Result");
            chart.Series["Result"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < 1000; i++)
            {
                DataPoint Dp = new DataPoint(net.dataSetX[i], net.resultDataSetY[i]);
                chart.Series["Result"].Points.Add(Dp);
            }
            chart.ChartAreas[0].RecalculateAxesScale();
            text.Text = net.ToString() + " Time = " + watch.ElapsedMilliseconds.ToString();
        }
    }
}
